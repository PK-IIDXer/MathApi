namespace MathApi.Models;

public class Theorem
{
  public long Id { get; set; }
  public string Name { get; set; } = "";
  public bool IsProved { get; set; } = false;

  public List<TheoremAssumption>? TheoremAssumptions { get; set; }
  public List<TheoremConclusion> TheoremConclusions { get; set; } = new();

  public List<Proof>? Proofs { get; } = new();
}
