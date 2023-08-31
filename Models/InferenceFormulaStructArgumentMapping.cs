using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(InferenceId), nameof(SerialNo))]
public class InferenceFormulaStructArgumentMapping
{
  public Inference Inference { get; } = new();
  public long InferenceId { get; set; }
  public int SerialNo { get; set; }
  public FormulaStructArgument? FormulaStructArgument { get; set; }
  public int FormulaStructId { get; set; }
  public int FormulaStructArgumentSerialNo { get; set; }
  public InferenceArgument? InferenceArgument { get; set; }
  public int InferenceArgumentSerialNo { get; set; }

  public List<InferenceAssumption> InferenceAssumptions { get; } = new();
  public List<InferenceAssumptionDissolutableAssumption> InferenceAssumptionDissolutableAssumptions { get; } = new();
  public List<InferenceConclusion> InferenceConclusions { get; } = new();
}
