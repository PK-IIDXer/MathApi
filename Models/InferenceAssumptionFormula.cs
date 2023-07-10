using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(InferenceId), nameof(InferenceAssumptionSerialNo), nameof(SerialNo))]
public class InferenceAssumptionFormula
{
  public InferenceAssumption InferenceAssumption { get; } = new();
  public long InferenceId { get; set; }
  public int InferenceAssumptionSerialNo { get; set; }
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

  public List<InferenceAssumptionDissolutableAssumptionFormula>? InferenceAssumptionDissolutableAssumptionFormulas { get; }
}