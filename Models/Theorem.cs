namespace MathApi.Models;

public class Theorem
{
  public long Id { get; set; }
  public string Name { get; set; } = null!;
  public bool IsProved { get; set; } = false;

  public List<TheoremAssumption>? TheoremAssumptions { get; set; }
  public List<TheoremConclusion> TheoremConclusions { get; set; } = new();

  public List<ProofHead>? ProofHeads { get; } = new();
}
