using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(AxiomId), nameof(SerialNo))]
public class AxiomProposition
{
  public Axiom Axiom { get; } = new();
  public long AxiomId { get; set; }
  public long SerialNo { get; set; }
  public Formula Formula { get; } = new();
  public long FormulaId { get; set; }
  public string? Meaning { get; set; }
}
