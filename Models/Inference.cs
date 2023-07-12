namespace MathApi.Models;

public class Inference
{
  public long Id { get; set; }
  public string Name { get; set;} = "";
  public bool IsAssumptionAdd { get; set; }

  public List<InferenceArgument>? InferenceArguments { get; set; }
  public List<InferenceAssumption>? InferenceAssumptions { get; set; }
  public List<InferenceConclusionFormula> InferenceConclusionFormulas { get; set; } = new();

  public Proof? Proof { get; }
}
