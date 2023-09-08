using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MathApi.Models;

[Index(nameof(Character), nameof(TypeId), IsUnique = true)]
public class Symbol
{
  public long Id { get; set; }
  public string Character { get; set; } = "";
  public SymbolType? Type { get; }
  public Const.SymbolTypeEnum TypeId { get; set; }
  public int? Arity { get; set; }
  public FormulaType? ArityFormulaType { get; }
  public Const.FormulaTypeEnum? ArityFormulaTypeId { get; set; }
  public string? Meaning { get; set; }

  public List<FormulaString>? FormulaStrings { get; }
  public List<FormulaStructString>? FormulaStructStrings { get; }

  [JsonIgnore]
  [NotMapped]
  public bool IsQuantifier
  {
    get => Type?.IsQuantifier ?? throw new ArgumentException("Include SymbolType");
  }
}
