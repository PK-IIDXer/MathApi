using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(FormulaId), nameof(SerialNo))]
public class FormulaString
{
  public Formula Formula { get; } = new();
  public long FormulaId { get; set; }
  public long SerialNo { get; set; }
  public Symbol Symbol { get; } = new();
  public long SymbolId { get; set; }

  public List<FormulaChain>? FormulaChains { get; }

  public List<InferenceAssumptionFormula>? InferenceAssumptionFormulas { get; }
  public List<InferenceConclusionFormula>? InferenceConclusionFormulas { get; }
  public List<InferenceAssumptionDissolutableAssumptionFormula>? InferenceAssumptionDissolutableAssumptionFormulas { get; }
}
