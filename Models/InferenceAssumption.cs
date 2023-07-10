using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(InferenceId), nameof(SerialNo))]
public class InferenceAssumption
{
  public Inference Inference { get; set; } = new();
  public long InferenceId { get; set; }
  public int SerialNo { get; set; }
  public InferenceAssumptionDissolutionType InferenceAssumptionDissolutionType { get; set; } = new();
  public int InferenceAssumptionDissolutionTypeId { get; set; }

  public List<InferenceArgumentConstraint>? InferenceArgumentConstraints { get; }
  public List<InferenceAssumptionFormula> InferenceAssumptionFormulas { get; } = new();
}
