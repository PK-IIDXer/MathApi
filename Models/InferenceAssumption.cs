using Microsoft.EntityFrameworkCore;
using MathApi.Commons;

namespace MathApi.Models;

/// <summary>
/// 推論規則仮定
/// </summary>
[PrimaryKey(nameof(InferenceId), nameof(SerialNo))]
public class InferenceAssumption
{
  public Inference Inference { get; set; } = new();
  /// <summary>
  /// 推論規則ID
  /// </summary>
  public long InferenceId { get; set; }
  /// <summary>
  /// 推論規則仮定連番
  /// </summary>
  public int SerialNo { get; set; }
  public InferenceAssumptionDissolutionType InferenceAssumptionDissolutionType { get; } = new();
  /// <summary>
  /// 推論規則仮定解消種類ID
  /// </summary>
  public int InferenceAssumptionDissolutionTypeId { get; set; }

  public List<InferenceAssumptionFormula> InferenceAssumptionFormulas { get; set; } = new();
  public List<InferenceAssumptionDissolutableAssumptionFormula>? InferenceAssumptionDissolutableAssumptionFormulas { get; set; }

  /// <summary>
  /// 仮定論理式を作成。
  /// ※以下のインクルードが必要
  /// ・Inference
  /// ・Inference.InferenceArguments
  /// ・InferenceAssumptionFormulas
  /// ・InferenceAssumptionFormulas.Symbol
  /// ・args(引数).Formula
  /// ・args(引数).Formula.FormulaStrings
  /// ・args(引数).Formula.FormulaStrings.Symbol
  /// ・args(引数).Formula.FormulaChains
  /// </summary>
  public Formula CreateAssumptionFormula(List<ProofInferenceArgument> args)
  {
    // 基本的な推論規則の場合
    if (Inference.IsBasic)
    {
      // 一文字目が記号の場合
      if (InferenceAssumptionFormulas[0].SymbolId != null)
      {
        var firstSymbol = InferenceAssumptionFormulas[0].Symbol;
        var isQuant = firstSymbol?.SymbolTypeId == (long)Const.SymbolType.TermQuantifier
                   || firstSymbol?.SymbolTypeId == (long)Const.SymbolType.PropositionQuantifier;

        // 一文字目が量化記号の場合
        // 推論規則結論文字列が[Q][x][A]の並びである前提で組み立てる
        if (isQuant)
        {
          var prop = args[InferenceAssumptionFormulas[2].SerialNo].Formula;
          // 代入操作が指示されている場合、代入を行う
          if (InferenceAssumptionFormulas[2].SubstitutionInferenceArgumentFromSerialNo.HasValue)
          {
            int fromSerialNo = InferenceAssumptionFormulas[2].SubstitutionInferenceArgumentFromSerialNo ?? throw new Exception();
            int toSerialNo = InferenceAssumptionFormulas[2].SubstitutionInferenceArgumentToSerialNo ?? throw new Exception();

            prop = prop.Substitute(
              args[fromSerialNo].Formula,
              args[toSerialNo].Formula
            );
          }

          return Util.ProceedFormulaConstruction(
            firstSymbol,
            args[InferenceAssumptionFormulas[1].SerialNo].Formula,
            new List<Formula> { prop }
          );
        }

        // 一文字目が量化記号以外の場合
        // 推論規則結論文字列に束縛先変数が現れない前提で組み立てる
        var argFormulas = new List<Formula>();
        for (var i = 1; i < InferenceAssumptionFormulas.Count; i++)
        {
          var prop = args[InferenceAssumptionFormulas[i].SerialNo].Formula;
          // 代入操作が指示されている場合、代入を行う
          if (InferenceAssumptionFormulas[i].SubstitutionInferenceArgumentFromSerialNo.HasValue)
          {
            int fromSerialNo = InferenceAssumptionFormulas[i].SubstitutionInferenceArgumentFromSerialNo ?? throw new Exception();
            int toSerialNo = InferenceAssumptionFormulas[i].SubstitutionInferenceArgumentToSerialNo ?? throw new Exception();

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
        // このelse句内では、InferenceAssumptionFormulasが命題型のもののみである前提で戻り値を組み立てる
        var prop = args[InferenceAssumptionFormulas[0].SerialNo].Formula;
        // 代入操作が指示されている場合、代入を行う
        if (InferenceAssumptionFormulas[0].SubstitutionInferenceArgumentFromSerialNo.HasValue)
        {
          int fromSerialNo = InferenceAssumptionFormulas[0].SubstitutionInferenceArgumentFromSerialNo ?? throw new Exception();
          int toSerialNo = InferenceAssumptionFormulas[0].SubstitutionInferenceArgumentToSerialNo ?? throw new Exception();

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
      var ret = InferenceAssumptionFormulas[0].Formula;
      for (var i = 0; i < args.Count; i++)
      {
        var fromFormula = new Formula
        {
          FormulaStrings = new List<FormulaString>
          {
            new FormulaString
            {
              SerialNo = 0,
              SymbolId = Inference.InferenceArguments[i].VariableSymbolId ?? throw new Exception()
            }
          }
        };
        var toFormula = args[i].Formula;
        ret = ret?.Substitute(fromFormula, toFormula);
      }
      return ret ?? throw new Exception();
    }
  }

  /// <summary>
  /// 除去可能仮定論理式を作成。
  /// ※以下のインクルードが必要
  /// ・Inference
  /// ・Inference.InferenceArguments
  /// ・InferenceAssumptionDissolutableAssumptionFormulas
  /// ・InferenceAssumptionDissolutableAssumptionFormulas.Symbol
  /// ・args(引数).Formula
  /// ・args(引数).Formula.FormulaStrings
  /// ・args(引数).Formula.FormulaStrings.Symbol
  /// ・args(引数).Formula.FormulaChains
  /// </summary>
  public Formula? CreateDissolutableAssumptionFormula(List<ProofInferenceArgument> args)
  {
    if (InferenceAssumptionDissolutableAssumptionFormulas == null)
      return null;

    // 基本的な推論規則の場合
    if (Inference.IsBasic)
    {
      // 一文字目が記号の場合
      if (InferenceAssumptionDissolutableAssumptionFormulas[0].SymbolId != null)
      {
        var firstSymbol = InferenceAssumptionDissolutableAssumptionFormulas[0].Symbol;
        var isQuant = firstSymbol?.SymbolTypeId is ((long)Const.SymbolType.TermQuantifier)
                   or ((long)Const.SymbolType.PropositionQuantifier);

        // 一文字目が量化記号の場合
        // 推論規則結論文字列が[Q][x][A]の並びである前提で組み立てる
        if (isQuant)
        {
          var prop = args[InferenceAssumptionDissolutableAssumptionFormulas[2].SerialNo].Formula;
          // 代入操作が指示されている場合、代入を行う
          if (InferenceAssumptionDissolutableAssumptionFormulas[2].SubstitutionInferenceArgumentFromSerialNo.HasValue)
          {
            int fromSerialNo = InferenceAssumptionDissolutableAssumptionFormulas[2].SubstitutionInferenceArgumentFromSerialNo ?? throw new Exception();
            int toSerialNo = InferenceAssumptionDissolutableAssumptionFormulas[2].SubstitutionInferenceArgumentToSerialNo ?? throw new Exception();

            prop = prop.Substitute(
              args[fromSerialNo].Formula,
              args[toSerialNo].Formula
            );
          }

          return Util.ProceedFormulaConstruction(
            firstSymbol,
            args[InferenceAssumptionDissolutableAssumptionFormulas[1].SerialNo].Formula,
            new List<Formula> { prop }
          );
        }

        // 一文字目が量化記号以外の場合
        // 推論規則結論文字列に束縛先変数が現れない前提で組み立てる
        var argFormulas = new List<Formula>();
        for (var i = 1; i < InferenceAssumptionDissolutableAssumptionFormulas.Count; i++)
        {
          var prop = args[InferenceAssumptionDissolutableAssumptionFormulas[i].SerialNo].Formula;
          // 代入操作が指示されている場合、代入を行う
          if (InferenceAssumptionDissolutableAssumptionFormulas[i].SubstitutionInferenceArgumentFromSerialNo.HasValue)
          {
            int fromSerialNo = InferenceAssumptionDissolutableAssumptionFormulas[i].SubstitutionInferenceArgumentFromSerialNo ?? throw new Exception();
            int toSerialNo = InferenceAssumptionDissolutableAssumptionFormulas[i].SubstitutionInferenceArgumentToSerialNo ?? throw new Exception();

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
        // このelse句内では、InferenceAssumptionDissolutableAssumptionFormulasが命題型のもののみである前提で戻り値を組み立てる
        var prop = args[InferenceAssumptionDissolutableAssumptionFormulas[0].SerialNo].Formula;
        // 代入操作が指示されている場合、代入を行う
        if (InferenceAssumptionDissolutableAssumptionFormulas[0].SubstitutionInferenceArgumentFromSerialNo.HasValue)
        {
          int fromSerialNo = InferenceAssumptionDissolutableAssumptionFormulas[0].SubstitutionInferenceArgumentFromSerialNo ?? throw new Exception();
          int toSerialNo = InferenceAssumptionDissolutableAssumptionFormulas[0].SubstitutionInferenceArgumentToSerialNo ?? throw new Exception();

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
      // 基本的な推論規則以外の場合、解消可能仮定は存在しない想定
      return null;
    }
  }
}
