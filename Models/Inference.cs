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
  /// 推論定理の場合、推論規則を定理ヘッダに紐づける
  /// </summary>
  public Theorem? Theorem { get; set; }
  /// <summary>
  /// 推論定理の場合、推論規則を定理ヘッダに紐づける
  /// </summary>
  public long? TheoremId { get; set; }

  public List<InferenceArgument> InferenceArguments { get; set; } = new();
  public List<InferenceAssumption> InferenceAssumptions { get; set; } = new();
  public List<InferenceConclusionFormula> InferenceConclusionFormulas { get; set; } = new();

  public List<ProofInference>? ProofInferences { get; }

  public struct InferenceResult
  {
    public Formula ConclusionFormula { get; set; }
    public List<ProofInference> UpdatedProofInferences { get; set; }
    public List<ProofAssumption> UpdatedProofAssumptions { get; set; }
    public ProofAssumption? AddedProofAssumption { get; set; }
  }

  public InferenceResult Apply(
    long nextProofInferenceSerialNo,
    List<ProofInference> prevProofInferences,
    List<ProofAssumption> proofAssumptions,
    List<ProofInferenceArgument> args)
  {
    InferenceResult result = new();

    // 基本的なバリデーション
    ValidateProofInference(args);

    if (prevProofInferences.Count != InferenceAssumptions.Count)
      throw new ArgumentException("Inference assumption count mismatch");

    // 未解消仮定の解消
    foreach (var ia in InferenceAssumptions)
    {
      var willDissolutable = ia.CreateDissolutableAssumptionFormula(args);

      // 仮定論理式の存在確認
      var assumptionFormula = ia.CreateAssumptionFormula(args);
      var checkedFlag = false;
      // 解消フラグ（解消必須の仮定が存在しない場合エラーとする処理に使用
      bool dissoluteFlag = false;
      foreach (var prevPi in prevProofInferences)
      {
        if (prevPi.NextProofInferenceSerialNo.HasValue)
          continue;
        if (prevPi.ConclusionFormula?.Equals(assumptionFormula) ?? throw new Exception())
        {
          prevPi.NextProofInferenceSerialNo = nextProofInferenceSerialNo;

          result.UpdatedProofInferences.Add(prevPi);
          checkedFlag = true;
        }
        if (!checkedFlag)
          continue;

        // 仮定の解消
        if (willDissolutable == null)
          continue;
        foreach (var pa in proofAssumptions.Where(p => prevPi.TreeFrom <= p.AddedProofInferenceSerialNo && p.AddedProofInferenceSerialNo <= prevPi.TreeTo))
        {
          if (pa.DissolutedProofInference != null)
            continue;

          if (willDissolutable.Equals(pa.Formula ?? throw new Exception()))
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

        if (checkedFlag)
          break;
      }
      if (!checkedFlag)
        throw new ArgumentException("Assumption mismatch");

      // 解消必須の仮定が存在しない場合エラー
      if (ia.InferenceAssumptionDissolutionTypeId == (int)Const.InferenceAssumptionDissolutionType.Required
        && dissoluteFlag == false)
      {
        throw new ArgumentException("dissolution assumption is required but is not entered.");
      }
    }

    // 推論規則制約のチェック
    ValidateInferenceConstraint(proofAssumptions, args);

    // 更新対象の返却
    result.ConclusionFormula = CreateConclusionFormula(args);
    if (IsAssumptionAdd)
    {
      result.AddedProofAssumption = new ProofAssumption()
      {
        Formula = args[0].Formula,
        FormulaId = args[0].FormulaId,
        AddedProofInferenceSerialNo = nextProofInferenceSerialNo,
        LastUsedProofInferenceSerialNo = nextProofInferenceSerialNo
      };
    }

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
        if (args[i].Formula?.FormulaTypeId != (long)Const.FormulaType.Term)
          throw new ArgumentException("argument formula mismatch");
      }
      if (InferenceArguments[i].InferenceArgumentTypeId == (int)Const.InferenceArgumentType.Proposition)
      {
        if (args[i].Formula?.FormulaTypeId != (long)Const.FormulaType.Proposition)
          throw new ArgumentException("argument formula mismatch");
      }
      if (InferenceArguments[i].InferenceArgumentTypeId == (int)Const.InferenceArgumentType.FreeVariable)
      {
        if (!args[i].Formula?.IsFreeVariable ?? throw new Exception())
          throw new ArgumentException("argument formula mismatch");
      }
    }

    // 結論に関するバリデーション
    if (InferenceConclusionFormulas.Count == 0)
      throw new ArgumentException("InferenceConclusionFormulas is null.");

    if (InferenceConclusionFormulas[0].SymbolId != null)
    {
      var firstSymbol = InferenceConclusionFormulas[0].Symbol;
      if (firstSymbol?.IsQuantifier ?? false)
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
        if (firstSymbol?.IsQuantifier ?? false)
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
        if (firstSymbol?.IsQuantifier ?? false)
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
        if (!arg.Formula?.IsFreeVariable ?? throw new Exception())
          throw new ArgumentException("only free variable is arrowed, where has Inference argument constraints");

        var trgInfArg = args.Find(a => a.SerialNo == iac.ConstraintDestinationInferenceArgumentSerialNo)
          ?? throw new ArgumentException("Proof Argument and Inference Argument are mismatch");

        if (trgInfArg.Formula?.HasSymbol(arg.Formula?.FormulaStrings[0].Symbol ?? throw new Exception()) ?? throw new Exception())
          throw new ArgumentException("Inference Argument Constraint Error");

        // 未解消仮定への制約チェック
        if (iac.IsConstraintPredissolvedAssumption)
        {
          foreach (var lastUsedProofInference in proofAssumptions.Where(pa => pa.LastUsedProofInferenceSerialNo == arg.ProofInference.SerialNo))
          {
            if (lastUsedProofInference.Formula?.HasSymbol(arg.Formula.FormulaStrings[0].Symbol) ?? throw new Exception())
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
    return Util.CreateProofInferenceFormula(InferenceConclusionFormulas.Select(icf => icf as IInferenceFormula).ToList(), args);
  }
}

public interface IInferenceFormula
{
  public Symbol? Symbol { get; }
  public long? SymbolId { get; set; }
  public int? BoundInferenceArgumentSerialNo { get; set; }
  public int? InferenceArgumentSerialNo { get; set; }
  public int? SubstitutionInferenceArgumentFromSerialNo { get; set; }
  public int? SubstitutionInferenceArgumentToSerialNo { get; set; }
}
