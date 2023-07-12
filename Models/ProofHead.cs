using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(Id), nameof(SerialNo))]
public class ProofHead
{
  public long Id { get; set; }
  public Theorem Theorem { get; } = new();
  public long TheoremId { get; set; }
  public long SerialNo { get; set; }

  public List<Proof>? Proofs { get; set; }
}
