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

  public InferenceAssumptionDissolutableAssumptionFormula? InferenceAssumptionDissolutableAssumptionFormulaToBound { get; }
  public InferenceAssumptionDissolutableAssumptionFormula? InferenceAssumptionDissolutableAssumptionFormula { get; }
  public InferenceAssumptionDissolutableAssumptionFormula? InferenceAssumptionDissolutableAssumptionFormulaToSubstitutionInferenceArgumentFrom { get; }
  public InferenceAssumptionDissolutableAssumptionFormula? InferenceAssumptionDissolutableAssumptionFormulaToSubstitutionInferenceArgumentTo { get; }

  public InferenceConclusionFormula? InferenceConclusionFormulaToBound { get; }
  public InferenceConclusionFormula? InferenceConclusionFormula { get; }
  public InferenceConclusionFormula? InferenceConclusionFormulaToSubstitutionInferenceArgumentFrom { get; }
  public InferenceConclusionFormula? InferenceConclusionFormulaToSubstitutionInferenceArgumentTo { get; }

  public InferenceAssumptionFormula? InferenceAssumptionFormulaToBound { get; }
  public InferenceAssumptionFormula? InferenceAssumptionFormula { get; }
  public InferenceAssumptionFormula? InferenceAssumptionFormulaToSubstitutionInferenceArgumentFrom { get; }
  public InferenceAssumptionFormula? InferenceAssumptionFormulaToSubstitutionInferenceArgumentTo { get; }

  public List<InferenceArgumentConstraint>? InferenceArgumentConstraints { get; }
  public List<InferenceArgumentConstraint>? InferenceArgumentConstraintDistinations { get; }
}
