using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(InferenceId), nameof(SerialNo))]
public class InferenceArgument
{
  public Inference Inference { get; } = new();
  public long InferenceId { get; set; }
  public int SerialNo { get; set; }
  public InferenceArgumentType InferenceArgumentType { get; } = new();
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
