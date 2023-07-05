namespace MathApi.Models;

public class Symbol
{
  public long Id { get; set; }
  public string Character { get; set; } = "";
  public SymbolType? SymbolType { get; set; }
  public long SymbolTypeId { get; set; }
  public int? Arity { get; set; }
  public FormulaType? ArityFormulaType { get; set; }
  public long? ArityFormulaTypeId { get; set; }
}
