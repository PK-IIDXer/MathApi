using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(TheoremId), nameof(ProofSerialNo), nameof(SerialNo))]
public class ProofArgument
{
  public Proof Proof { get; } = new();
  public long TheoremId { get; set; }
  public long ProofSerialNo { get; set; }
  public long SerialNo { get; set; }
  public AxiomProposition? AxiomProposition { get; }
  public long? AxiomId { get; set; }
  public long? AxiomPropositionSerialNo { get; set; }
  public TheoremAssumption? TheoremAssumption { get; }
  public long? TheoremAssumptionSerialNo { get; set; }
  public Formula Formula { get; } = new();
  public long FormulaId { get; set; }
}
