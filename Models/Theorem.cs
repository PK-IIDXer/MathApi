namespace MathApi.Models;

public class Theorem
{
  public long Id { get; set; }
  public string Name { get; set; } = null!;
  public bool IsProved { get; set; } = false;

  public List<TheoremAssumption>? TheoremAssumptions { get; }
  public List<TheoremConclusion> TheoremConclusions { get; } = new();

  public List<ProofHead>? ProofHeads { get; } = new();
}
