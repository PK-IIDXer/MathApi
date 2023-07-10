using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(InferenceId), nameof(SerialNo))]
public class InferenceConclusionFormula
{
  public Inference Inference { get; } = new();
  public long InferenceId { get; set; }
  public int SerialNo { get; set; }
  public Symbol? Symbol { get; }
  public long? SymbolId { get; set; }
  public InferenceArgument? BoundInferenceArgument { get; }
  public int BoundInferenceArgumentSerialNo { get; set; }
  public InferenceArgument? InferenceArgument { get; }
  public int InferenceArgumentSerialNo { get; set; }
  public InferenceArgument? SubstitutionInferenceArgumentFrom { get; }
  public int SubstitutionInferenceArgumentFromSerialNo { get; set; }
  public InferenceArgument? SubstitutionInferenceArgumentTo { get; }
  public int SubstitutionInferenceArgumentToSerialNo { get; set; }
}
