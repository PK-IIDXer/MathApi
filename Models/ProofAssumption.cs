using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(ProofHeadId), nameof(ProofHeadSerialNo), nameof(ProofSerialNo), nameof(SerialNo))]
public class ProofAssumption
{
  public Proof Proof { get; } = new();
  public long ProofHeadId { get; set; }
  public long ProofHeadSerialNo { get; set; }
  public long ProofSerialNo { get; set; }
  public long SerialNo { get; set; }
  public Formula Formula { get; } = new();
  public long FormulaId { get; set; }
  public long? DissolutedProofSerialNo { get; set; }
}
