using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(InferenceId), nameof(InferenceAssumptionSerialNo), nameof(SerialNo))]
public class InferenceAssumptionDissolutableAssumptionFormula
{
  public InferenceAssumption InferenceAssumption { get; } = new();
  public long InferenceId { get; set; }
  public int InferenceAssumptionSerialNo { get; set; }
  public int SerialNo { get; set; }
  public Symbol Symbol { get; } = new();
  public long? SymbolId { get; set; }
  public InferenceArgument BoundInferenceArgument { get; } = new();
  public int? BoundInferenceArgumentSerialNo { get; set; }
  public InferenceArgument InferenceArgument { get; } = new();
  public int? InferenceArgumentSerialNo { get; set; }
  public InferenceArgument SubstitutionInferenceArgumentFrom { get; } = new();
  public int? SubstitutionInferenceArgumentFromSerialNo { get; set; }
  public InferenceArgument SubstitutionInferenceArgumentTo { get; } = new();
  public int? SubstitutionInferenceArgumentToSerialNo { get; set; }
}