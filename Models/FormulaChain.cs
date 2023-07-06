using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(FormulaId), nameof(SerialNo))]
public class FormulaChain
{
  public long FormulaId { get; set; }
  public long SerialNo { get; set; }
  public long FromFormulaStringSerialNo { get; set; }
  public long ToFormulaStringSerialNo { get; set; }
}