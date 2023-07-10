namespace MathApi.Models;

public class InferenceAssumptionDissolutionType
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;

  public List<InferenceAssumption>? InferenceAssumptions { get; }
}
