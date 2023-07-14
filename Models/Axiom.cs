namespace MathApi.Models;

public class Axiom
{
  public long Id { get; set; }
  public string Name { get; set; } = "";
  public string? Remarks { get; set; }

  public List<AxiomProposition> AxiomPropositions { get; set; } = new();
}
