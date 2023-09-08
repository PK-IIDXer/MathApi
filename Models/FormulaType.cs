namespace MathApi.Models;

public class FormulaType
{
  public Const.FormulaType Id { get; set; }
  public string Name { get; set; } = "";

  public List<SymbolType>? SymbolTypes { get; }
  public List<Symbol>? Symbols { get; }
}
