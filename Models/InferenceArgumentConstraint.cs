using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(InferenceId), nameof(InferenceArgumentSerialNo), nameof(SerialNo))]
public class InferenceArgumentConstraint
{
  public InferenceArgument InferenceArgument { get; } = new();
  public long InferenceId { get; set; }
  public int InferenceArgumentSerialNo { get; set; }
  public int SerialNo { get; set; }
  public InferenceArgument ConstraintDestinationInferenceArgument { get; } = new();
  public int ConstraintDestinationInferenceArgumentSerialNo { get; set; }
  public bool IsConstraintPredissolvedAssumption { get; set; }
}
