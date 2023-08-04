using MathApi.Commons;

namespace MathApi.Models;

/// <summary>
/// 推論規則
/// </summary>
public class Inference
{
  /// <summary>
  /// 推論規則ID
  /// </summary>
  public long Id { get; set; }
  /// <summary>
  /// 推論規則名称
  /// </summary>
  public string Name { get; set;} = "";
  /// <summary>
  /// 仮定を追加するかどうか
  /// </summary>
  public bool IsAssumptionAdd { get; set; }
  /// <summary>
  /// 基礎的な推論規則かどうか
  /// </summary>
  /// <remarks>
  /// 基礎的な推論規則とは以下の推論規則のことである:
  /// ∧導入、∧除去(左)、∧除去(右)、∨導入(左)、∨導入(右)、∨除去、
  /// →導入、→除去、￢導入、￢除去、背理法、矛盾、
  /// ∀導入、∀除去、∃導入、∃除去、等号公理、等号規則。
  /// これら以外の推論規則は論理式によって定義され、各仮定・結論は論理式IDのみを参照する。
  /// このとき引数は、仮定・結論の論理式に現れるすべての自由変数・命題変数とする。
  /// </remarks>
  public bool IsBasic { get; set; }
  /// <summary>
  /// 証明済みの定理が推論規則として登録されたときに使用。
  /// </summary>
  public Theorem? Theorem { get; set; }
  /// <summary>
  /// 証明済みの定理が推論規則として登録されたときに使用。
  /// </summary>
  public long? TheoremId { get; set; }

  public List<InferenceArgument> InferenceArguments { get; set; } = new();
  public List<InferenceAssumption> InferenceAssumptions { get; set; } = new();
  public List<InferenceConclusionFormula> InferenceConclusionFormulas { get; set; } = new();

  public List<ProofInference>? ProofInferences { get; }

  public struct InferenceResult
  {
    public ProofInference ProofInference { get; set; }
    public List<ProofInference> UpdatedProofInferences { get; set; }
    public List<ProofAssumption> UpdatedProofAssumptions { get; set; }
  }

  public InferenceResult Apply(
    long nextProofInferenceSerialNo,
    Proof proof,
    List<ProofInference> prevProofInferences,
    List<ProofAssumption> proofAssumptions,
    List<ProofInference> unusedProofInferences,
    List<ProofInferenceArgument> args)
  {
    InferenceResult result = new();

    // 基本的なバリデーション
    ValidateProofInference(args);
    var prevConclusionFormulas = prevProofInferences.Select(ppi => ppi.ConclusionFormula).ToList();
    if ((prevConclusionFormulas?.Count ?? 0) != InferenceAssumptions.Count)
      throw new ArgumentException("Inference assumption count mismatch");

    // 未解消仮定の解消
    foreach (var ia in InferenceAssumptions)
    {
      var willDissolutable = ia.CreateDissolutableAssumptionFormula(args);
      if (willDissolutable == null)
        continue;

      // 解消フラグ（解消必須の仮定が存在しない場合エラーとする処理に使用
      bool dissoluteFlag = false;
      foreach (var pa in proofAssumptions)
      {
        if (pa.DissolutedProofInference != null)
          continue;

        if (willDissolutable.Equals(pa.Formula))
        {
          pa.DissolutedProofInferenceSerialNo = nextProofInferenceSerialNo;
          pa.LastUsedProofInferenceSerialNo = nextProofInferenceSerialNo;
          dissoluteFlag = true;

          result.UpdatedProofAssumptions.Add(pa);
          break;
        }
      }

      // 解消必須の仮定が存在しない場合エラー
      if (ia.InferenceAssumptionDissolutionTypeId == (int)Const.InferenceAssumptionDissolutionType.Required
        && dissoluteFlag == false)
      {
        throw new ArgumentException("dissolution assumption is required but is not entered.");
      }
    }

    // 推論規則制約のチェック
    ValidateInferenceConstraint(proofAssumptions, args);

    // 仮定論理式のチェック
    foreach (var ia in InferenceAssumptions)
    {
      var assumptionFormula = ia.CreateAssumptionFormula(args);
      var checkedFlag = false;
      foreach (var unusedPi in unusedProofInferences)
      {
        if (unusedPi.ConclusionFormula.Equals(assumptionFormula))
        {
          unusedPi.NextProofInferenceSerialNo = nextProofInferenceSerialNo;

          result.UpdatedProofInferences.Add(unusedPi);
          checkedFlag = true;
          break;
        }
      }
      if (!checkedFlag)
        throw new ArgumentException("Assumption mismatch");
    }

    // 結論論理式の作成
    var conclusion = CreateConclusionFormula(args);

    // 更新対象の返却
    result.ProofInference = new ProofInference
    {
      TheoremId = proof.TheoremId,
      ProofSerialNo = proof.SerialNo,
      SerialNo = nextProofInferenceSerialNo,
      InferenceId = Id,
      ConclusionFormula = CreateConclusionFormula(args),
      PreviousProofInferences = prevProofInferences
    };

    return result;
  }

  private void ValidateProofInference(List<ProofInferenceArgument> args)
  {
    if (args.Count != InferenceArguments.Count)
      throw new ArgumentException("args count mismatch");

    if (args.Count == 0)
      return;

    for (var i = 0; i < args.Count; i++)
    {
      if (InferenceArguments[i].InferenceArgumentTypeId == (int)Const.InferenceArgumentType.Term)
      {
        if (args[i].Formula.FormulaTypeId != (long)Const.FormulaType.Term)
          throw new ArgumentException("argument formula mismatch");
      }
      if (InferenceArguments[i].InferenceArgumentTypeId == (int)Const.InferenceArgumentType.Proposition)
      {
        if (args[i].Formula.FormulaTypeId != (long)Const.FormulaType.Proposition)
          throw new ArgumentException("argument formula mismatch");
      }
      if (InferenceArguments[i].InferenceArgumentTypeId == (int)Const.InferenceArgumentType.FreeVariable)
      {
        if (!args[i].Formula.IsFreeVariable)
          throw new ArgumentException("argument formula mismatch");
      }
    }

    // 結論に関するバリデーション
    if (InferenceConclusionFormulas.Count == 0)
      throw new ArgumentException("InferenceConclusionFormulas is null.");

    if (InferenceConclusionFormulas[0].SymbolId != null)
    {
      var firstSymbol = InferenceConclusionFormulas[0].Symbol;
      var isQuant = firstSymbol?.SymbolTypeId == (long)Const.SymbolType.TermQuantifier
                 || firstSymbol?.SymbolTypeId == (long)Const.SymbolType.PropositionQuantifier;

      if (isQuant)
      {
        if (InferenceConclusionFormulas.Count != 3)
          throw new ArgumentException("InferenceConclusionFormulas should be 3-formulas if first symbol is quantifier");
      }
    }

    foreach (var inferenceAssumption in InferenceAssumptions)
    {
      // 仮定に関するバリデーション
      var assumptionFormulas = inferenceAssumption.InferenceAssumptionFormulas;
      if (assumptionFormulas.Count == 0)
        throw new ArgumentException("InferenceAssumptionFormulas is null.");
      if (assumptionFormulas[0].SymbolId != null)
      {
        var firstSymbol = assumptionFormulas[0].Symbol;
        var isQuant = firstSymbol?.SymbolTypeId == (long)Const.SymbolType.TermQuantifier
                   || firstSymbol?.SymbolTypeId == (long)Const.SymbolType.PropositionQuantifier;
        if (isQuant)
        {
          if (assumptionFormulas.Count != 3)
            throw new ArgumentException("InferenceAssumptionFormulas should be 3-formulas if first symbol is quantifier");
        }
      }

      // 解消可能仮定に関するバリデーション
      var dissolutableAssumptionFormulas = inferenceAssumption.InferenceAssumptionDissolutableAssumptionFormulas;
      if (dissolutableAssumptionFormulas == null)
        return;
      if (dissolutableAssumptionFormulas.Count == 0)
        throw new ArgumentException("InferenceAssumptionDissolutableAssumptionFormulas is null.");
      if (dissolutableAssumptionFormulas[0].SymbolId != null)
      {
        var firstSymbol = dissolutableAssumptionFormulas[0].Symbol;
        var isQuant = firstSymbol?.SymbolTypeId == (long)Const.SymbolType.TermQuantifier
                   || firstSymbol?.SymbolTypeId == (long)Const.SymbolType.PropositionQuantifier;
        if (isQuant)
        {
          if (dissolutableAssumptionFormulas.Count != 3)
            throw new ArgumentException("InferenceAssumptionDissolutableAssumptionFormulas should be 3-formulas if first symbol is quantifier");
        }
      }
    }
  }

  private void ValidateInferenceConstraint(List<ProofAssumption> proofAssumptions, List<ProofInferenceArgument> args)
  {
    foreach (var ia in InferenceArguments)
    {
      if (ia.InferenceArgumentConstraints == null)
        continue;

      var arg = args.Find(a => a.SerialNo == ia.SerialNo)
        ?? throw new ArgumentException("Proof Argument and Inference Argument are mismatch");

      foreach (var iac in ia.InferenceArgumentConstraints)
      {
        // 推論規則制約が存在する推論規則引数には、自由変数のみが許可される
        if (!arg.Formula.IsFreeVariable)
          throw new ArgumentException("only free variable is arrowed, where has Inference argument constraints");

        var trgInfArg = args.Find(a => a.SerialNo == iac.ConstraintDestinationInferenceArgumentSerialNo)
          ?? throw new ArgumentException("Proof Argument and Inference Argument are mismatch");

        if (trgInfArg.Formula.HasSymbol(arg.Formula.FormulaStrings[0].Symbol))
          throw new ArgumentException("Inference Argument Constraint Error");

        // 未解消仮定への制約チェック
        if (iac.IsConstraintPredissolvedAssumption)
        {
          foreach (var lastUsedProofInference in proofAssumptions.Where(pa => pa.LastUsedProofInferenceSerialNo == arg.ProofInference.SerialNo))
          {
            if (lastUsedProofInference.Formula.HasSymbol(arg.Formula.FormulaStrings[0].Symbol))
              throw new ArgumentException("Inference Argument Constraint Error");
          }
        }
      }
    }
  }

  /// <summary>
  /// 結論論理式を作成。
  /// ※以下のインクルードが必要
  /// ・InferenceConclusionFormulas
  /// ・InferenceConclusionFormulas.Symbol
  /// ・args(引数).Formula
  /// ・args(引数).Formula.FormulaStrings
  /// ・args(引数).Formula.FormulaStrings.Symbol
  /// ・args(引数).Formula.FormulaChains
  /// </summary>
  public Formula CreateConclusionFormula(List<ProofInferenceArgument> args)
  {
    // 基本的な推論規則の場合
    if (IsBasic)
    {
      // 結論の一文字目が記号の場合
      if (InferenceConclusionFormulas[0].SymbolId != null)
      {
        var firstSymbol = InferenceConclusionFormulas[0].Symbol;
        var isQuant = firstSymbol?.SymbolTypeId == (long)Const.SymbolType.TermQuantifier
                   || firstSymbol?.SymbolTypeId == (long)Const.SymbolType.PropositionQuantifier;

        // 一文字目が量化記号の場合
        // 推論規則結論文字列が[Q][x][A]の並びである前提で組み立てる
        if (isQuant)
        {
          var prop = args[InferenceConclusionFormulas[2].SerialNo].Formula;
          // 代入操作が指示されている場合、代入を行う
          if (InferenceConclusionFormulas[2].SubstitutionInferenceArgumentFromSerialNo.HasValue)
          {
            int fromSerialNo = InferenceConclusionFormulas[2].SubstitutionInferenceArgumentFromSerialNo ?? throw new Exception(Name);
            int toSerialNo = InferenceConclusionFormulas[2].SubstitutionInferenceArgumentToSerialNo ?? throw new Exception(Name);

            prop = prop.Substitute(
              args[fromSerialNo].Formula,
              args[toSerialNo].Formula
            );
          }

          return Util.ProceedFormulaConstruction(
            firstSymbol,
            args[InferenceConclusionFormulas[1].SerialNo].Formula,
            new List<Formula> { prop }
          );
        }

        // 一文字目が量化記号以外の場合
        // 推論規則結論文字列に束縛先変数が現れない前提で組み立てる
        var argFormulas = new List<Formula>();
        for (var i = 1; i < InferenceConclusionFormulas.Count; i++)
        {
          var prop = args[InferenceConclusionFormulas[i].SerialNo].Formula;
          // 代入操作が指示されている場合、代入を行う
          if (InferenceConclusionFormulas[i].SubstitutionInferenceArgumentFromSerialNo.HasValue)
          {
            int fromSerialNo = InferenceConclusionFormulas[i].SubstitutionInferenceArgumentFromSerialNo ?? throw new Exception(Name);
            int toSerialNo = InferenceConclusionFormulas[i].SubstitutionInferenceArgumentToSerialNo ?? throw new Exception(Name);

            prop = prop.Substitute(
              args[fromSerialNo].Formula,
              args[toSerialNo].Formula
            );
          }

          argFormulas.Add(prop);
        }
        return Util.ProceedFormulaConstruction(
          firstSymbol,
          null,
          argFormulas
        );
      }
      // 結論の一文字目が記号でない場合
      else
      {
        // このelse句内では、InferenceConclusionFormulasが命題型のもののみである前提で戻り値を組み立てる
        var prop = args[InferenceConclusionFormulas[0].SerialNo].Formula;
        // 代入操作が指示されている場合、代入を行う
        if (InferenceConclusionFormulas[0].SubstitutionInferenceArgumentFromSerialNo.HasValue)
        {
          int fromSerialNo = InferenceConclusionFormulas[0].SubstitutionInferenceArgumentFromSerialNo ?? throw new Exception(Name);
          int toSerialNo = InferenceConclusionFormulas[0].SubstitutionInferenceArgumentToSerialNo ?? throw new Exception(Name);

          prop = prop.Substitute(
            args[fromSerialNo].Formula,
            args[toSerialNo].Formula
          );
        }
        return prop;
      }
    }
    // 基本的な推論規則以外の場合
    else
    {
      // このelse句内では、論理式による推論規則の定義が行われている前提とする
      var ret = InferenceConclusionFormulas[0].Formula;
      for (var i = 0; i < args.Count; i++)
      {
        var fromFormula = new Formula
        {
          FormulaStrings = new List<FormulaString>
          {
            new FormulaString
            {
              SerialNo = 0,
              SymbolId = InferenceArguments[i].VariableSymbolId ?? throw new Exception(Name)
            }
          }
        };
        var toFormula = args[i].Formula;
        ret = ret?.Substitute(fromFormula, toFormula);
      }
      return ret ?? throw new Exception(Name);
    }
  }
}
