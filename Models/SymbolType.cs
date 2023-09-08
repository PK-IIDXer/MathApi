namespace MathApi.Models;

public class SymbolType
{
  public Const.SymbolType Id { get; set; }
  public string Name { get; set; } = "";
  public FormulaType FormulaType { get; } = new();
  public Const.FormulaType FormulaTypeId { get; set; }
  public bool IsQuantifier { get; set; }

  public List<Symbol>? Symbols { get; }
}
