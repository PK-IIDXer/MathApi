using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace MathApi.Models;

[PrimaryKey(nameof(InferenceId), nameof(SerialNo), nameof(FormulaStructId), nameof(FormulaStructArgumentSerialNo))]
public class InferenceFormulaStructArgumentMapping
{
  public Inference Inference { get; } = new();
  public long InferenceId { get; set; }
  public int SerialNo { get; set; }
  public FormulaStructArgument? FormulaStructArgument { get; set; }
  public long FormulaStructId { get; set; }
  public int FormulaStructArgumentSerialNo { get; set; }
  public InferenceArgument? InferenceArgument { get; set; }
  public int InferenceArgumentSerialNo { get; set; }

  [JsonIgnore]
  public InferenceAssumption? InferenceAssumption { get; }
  [JsonIgnore]
  public InferenceAssumptionDissolutableAssumption? InferenceAssumptionDissolutableAssumption { get; }
  [JsonIgnore]
  public InferenceConclusion? InferenceConclusion { get; }
}
