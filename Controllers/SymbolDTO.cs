using MathApi.Models;

namespace MathApi.Controllers;

public class SymbolDto
{
  public long Id { get; set; }
  public string Character { get; set; } = "";
  public long SymbolTypeId { get; set; }
  public int? Arity { get; set; }
  public long? ArityFormulaTypeId { get; set; }
  public string Meaning { get; set; } = "";

  public Symbol CreateModel()
  {
    return new Symbol
    {
      Id = Id,
      Character = Character,
      SymbolTypeId = SymbolTypeId,
      Arity = Arity,
      ArityFormulaTypeId = ArityFormulaTypeId,
      Meaning = Meaning
    };
  }
}
