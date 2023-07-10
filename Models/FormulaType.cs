namespace MathApi.Models;

public class FormulaType
{
  public long Id { get; set; }
  public string Name { get; set; } = "";

  public List<SymbolType>? SymbolTypes { get; }
  public List<Symbol>? Symbols { get; }
}
