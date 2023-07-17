using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(TheoremId), nameof(SerialNo))]
public class Proof
{
  public Theorem Theorem { get; } = new();
  public long TheoremId { get; set; }
  public long SerialNo { get; set; }

  public List<ProofInference>? ProofInferences { get; set; }
  public List<ProofAssumption>? ProofAssumptions { get; set; }
}
