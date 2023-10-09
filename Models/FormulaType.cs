using System.Text.Json.Serialization;

namespace MathApi.Models;

public class FormulaType
{
  public Const.FormulaTypeEnum Id { get; set; }
  public string Name { get; set; } = "";

  [JsonIgnore]
  public List<SymbolType>? SymbolTypes { get; }
  [JsonIgnore]
  public List<Symbol>? Symbols { get; }
}
