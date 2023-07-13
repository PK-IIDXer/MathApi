using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(FormulaId), nameof(SerialNo))]
public class FormulaString
{
  public Formula Formula { get; } = new();
  public long FormulaId { get; set; }
  public long SerialNo { get; set; }
  public Symbol Symbol { get; set; } = new();
  public long SymbolId { get; set; }

  public FormulaChain? FormulaChainAtFrom { get; }
  public FormulaChain? FormulaChainAtTo { get; }
}
