using System.Text.Json.Serialization;

namespace MathApi.Models;

public class SymbolType
{
  public Const.SymbolTypeEnum Id { get; set; }
  public string Name { get; set; } = "";
  [JsonIgnore]
  public FormulaType FormulaType { get; } = new();
  public Const.FormulaTypeEnum FormulaTypeId { get; set; }
  public bool IsQuantifier { get; set; }

  [JsonIgnore]
  public List<Symbol>? Symbols { get; }
}
