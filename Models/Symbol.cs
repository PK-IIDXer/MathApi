using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[Index(nameof(Character), nameof(SymbolTypeId), IsUnique = true)]
public class Symbol
{
  public long Id { get; set; }
  public string Character { get; set; } = "";
  public SymbolType SymbolType { get; } = new();
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
}
