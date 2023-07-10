using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(InferenceId), nameof(InferenceAssumptionSerialNo), nameof(SerialNo))]
public class InferenceArgumentConstraint
{
  public InferenceAssumption InferenceAssumption { get; } = new();
  public long InferenceId { get; set; }
  public int InferenceAssumptionSerialNo { get; set; }
  public int SerialNo { get; set; }
  public InferenceArgument ConstraintDestinationInferenceArgument { get; } = new();
  public int ConstraintDestinationInferenceArgumentSerialNo { get; set; }
  public bool IsConstraintPredissolvedAssumption { get; set; }
}
