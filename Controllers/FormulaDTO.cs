namespace MathApi.Controllers;

public class FormulaDto
{
  public long FirstSymbolId { get; set; }
  public List<long> ArgumentedFormulaIds { get; set; } = new List<long>();
  public long? BoundVariableId { get; set; }
  public string? Meaning { get; set; }
}