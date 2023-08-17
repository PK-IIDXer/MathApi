using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(FormulaStructId), nameof(FormulaStructStringSerialNo))]
public class FormulaStructStringSubstitution
{
  public FormulaStructString FormulaStructString { get; set; } = new();
  public long FormulaStructId { get; set; }
  public int FormulaStructStringSerialNo { get; set; }
  public int SerialNo { get; set; }
  public FormulaStructArgument? ArgumentFrom { get; set; }
  public int ArgumentFromSerialNo { get; set; }
  public FormulaStructArgument? ArgumentTo { get; set; }
  public int ArgumentToSerialNo { get; set; }
}
