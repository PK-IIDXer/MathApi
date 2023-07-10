namespace MathApi.Models;

public class SymbolType
{
  public long Id { get; set; }
  public string Name { get; set; } = "";
  public FormulaType FormulaType { get; } = new();
  public long FormulaTypeId { get; set; }

  public List<Symbol>? Symbols { get; }
}
