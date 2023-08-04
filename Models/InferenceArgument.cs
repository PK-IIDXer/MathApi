using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

/// <summary>
/// 推論規則引数
/// </summary>
/// <remarks>
/// 推論規則を適用する際にアサインする命題・項・自由変数・束縛変数を定義する。
/// 自由変数への項の代入操作が必要な場合(∃除去等)、その代入元・代入先も指定する。
/// </remarks>
[PrimaryKey(nameof(InferenceId), nameof(SerialNo))]
public class InferenceArgument
{
  public Inference Inference { get; } = new();
  /// <summary>
  /// 推論規則ID
  /// </summary>
  public long InferenceId { get; set; }
  /// <summary>
  /// 推論規則引数連番
  /// </summary>
  public int SerialNo { get; set; }
  public InferenceArgumentType InferenceArgumentType { get; } = new();
  /// <summary>
  /// 推論規則引数種類ID
  /// </summary>
  public int InferenceArgumentTypeId { get; set; }
  /// <summary>
  /// IsBasic = falseの場合に、論理式で推論規則を定義する場合に、
  /// 自由変数・命題変数を保持するために使用
  /// </summary>
  public Symbol? VariableSymbol { get; set; }
  /// <summary>
  /// IsBasic = falseの場合に、論理式で推論規則を定義する場合に、
  /// 自由変数・命題変数を保持するために使用
  /// </summary>
  public long? VariableSymbolId { get; set; }

  public List<InferenceAssumptionDissolutableAssumptionFormula>? InferenceAssumptionDissolutableAssumptionFormulasToBound { get; }
  public List<InferenceAssumptionDissolutableAssumptionFormula>? InferenceAssumptionDissolutableAssumptionFormulas { get; }
  public List<InferenceAssumptionDissolutableAssumptionFormula>? InferenceAssumptionDissolutableAssumptionFormulasToSubstitutionInferenceArgumentFrom { get; }
  public List<InferenceAssumptionDissolutableAssumptionFormula>? InferenceAssumptionDissolutableAssumptionFormulasToSubstitutionInferenceArgumentTo { get; }

  public List<InferenceConclusionFormula>? InferenceConclusionFormulasToBound { get; }
  public List<InferenceConclusionFormula>? InferenceConclusionFormulas { get; }
  public List<InferenceConclusionFormula>? InferenceConclusionFormulasToSubstitutionInferenceArgumentFrom { get; }
  public List<InferenceConclusionFormula>? InferenceConclusionFormulasToSubstitutionInferenceArgumentTo { get; }

  public List<InferenceAssumptionFormula>? InferenceAssumptionFormulasToBound { get; }
  public List<InferenceAssumptionFormula>? InferenceAssumptionFormulas { get; }
  public List<InferenceAssumptionFormula>? InferenceAssumptionFormulasToSubstitutionInferenceArgumentFrom { get; }
  public List<InferenceAssumptionFormula>? InferenceAssumptionFormulasToSubstitutionInferenceArgumentTo { get; }

  public List<InferenceArgumentConstraint>? InferenceArgumentConstraints { get; set; }
  public List<InferenceArgumentConstraint>? InferenceArgumentConstraintDistinations { get; }
}
