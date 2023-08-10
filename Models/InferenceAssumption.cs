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
  public InferenceAssumptionDissolutionType? InferenceAssumptionDissolutionType { get; }
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
    return Util.CreateProofInferenceFormula(InferenceAssumptionFormulas.Select(iaf => iaf as IInferenceFormula).ToList(), args);
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
      
    return Util.CreateProofInferenceFormula(InferenceAssumptionDissolutableAssumptionFormulas.Select(iadaf => iadaf as IInferenceFormula).ToList(), args);
  }
}
