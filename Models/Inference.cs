namespace MathApi.Models;

public class Inference
{
  public long Id { get; set; }
  public string Name { get; set;} = "";
  public bool IsAssumptionAdd { get; set; }

  public List<InferenceArgument>? InferenceArguments { get; }
  public List<InferenceAssumption>? InferenceAssumptions { get; }
  public List<InferenceConclusionFormula> InferenceConclusionFormulas { get; } = new();
}
