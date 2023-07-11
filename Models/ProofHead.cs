using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(TheoremId), nameof(SerialNo))]
public class ProofHead
{
  public Theorem Theorem { get; } = new();
  public long TheoremId { get; set; }
  public long SerialNo { get; set; }

  public List<Proof>? Proofs { get; }
}
