namespace MathApi.Models;

public class Formula
{
  public long Id { get; set; }
  public string? Meaning { get; set; }
  public List<FormulaString> FormulaStrings { get; set; } = new List<FormulaString>();
  public List<FormulaChain> FormulaChains { get; set; } = new List<FormulaChain>();

  public long Length => FormulaStrings.Count;
  public long? FormulaTypeId => FormulaStrings.Count == 0 ? null : FormulaStrings[0].Symbol.SymbolType?.FormulaType.Id;
}