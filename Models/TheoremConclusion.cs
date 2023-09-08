using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(TheoremId), nameof(SerialNo))]
public class TheoremConclusion
{
  public Theorem Theorem { get; } = new();
  public long TheoremId { get; set; }
  public long SerialNo { get; set; }
  public Formula? Formula { get; }
  public long FormulaId { get; set; }
}
