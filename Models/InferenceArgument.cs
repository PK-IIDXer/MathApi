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

  public List<InferenceArgumentConstraint>? InferenceArgumentConstraints { get; }
  public List<InferenceConclusionFormula>? InferenceConclusionFormulas { get; }
  public List<InferenceAssumptionFormula>? InferenceAssumptionFormulas { get; }
  public List<InferenceAssumptionDissolutableAssumptionFormula>? InferenceAssumptionDissolutableAssumptionFormulas { get; }
}
