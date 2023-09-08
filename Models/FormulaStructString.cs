using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(FormulaStructId), nameof(SerialNo))]
public class FormulaStructString
{
  public FormulaStruct FormulaStruct { get; set; } = new();
  public long FormulaStructId { get; set; }
  public int SerialNo { get; set; }
  public Symbol? Symbol { get; set; }
  public long? SymbolId { get; set; }
  public FormulaStructArgument? BoundArgument { get; set; }
  public int? BoundArgumentSerialNo { get; set; }
  public FormulaStructArgument? Argument { get; set; }
  public int? ArgumentSerialNo { get; set; }

  public List<FormulaStructStringSubstitution> Substitutions { get; set; } = new();
}
