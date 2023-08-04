using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MathApi.Models;

[Index(nameof(Character), nameof(SymbolTypeId), IsUnique = true)]
public class Symbol
{
  public long Id { get; set; }
  public string Character { get; set; } = "";
  public SymbolType? SymbolType { get; }
  public long SymbolTypeId { get; set; }
  public int? Arity { get; set; }
  public FormulaType? ArityFormulaType { get; }
  public long? ArityFormulaTypeId { get; set; }
  public string? Meaning { get; set; }

  public List<FormulaString>? FormulaStrings { get; }

  public List<InferenceConclusionFormula>? InferenceConclusionFormulas { get; }
  public List<InferenceAssumptionFormula>? InferenceAssumptionFormulas { get; }
  public List<InferenceAssumptionDissolutableAssumptionFormula>? InferenceAssumptionDissolutableAssumptionFormulas { get; }
  public List<InferenceArgument>? InferenceArguments { get; }

  [JsonIgnore]
  [NotMapped]
  public bool IsQuantifier
  {
    get => SymbolType?.IsQuantifier ?? throw new ArgumentException("Include SymbolType");
  }
}
