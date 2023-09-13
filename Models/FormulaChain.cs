using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(FormulaId), nameof(SerialNo))]
public class FormulaChain
{
  public Formula Formula { get; } = new();
  public long FormulaId { get; set; }
  public long SerialNo { get; set; }
  public FormulaString? FromFormulaString { get; }
  public long FromFormulaStringSerialNo { get; set; }
  public FormulaString? ToFormulaString { get; }
  public long ToFormulaStringSerialNo { get; set; }
}
