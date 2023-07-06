namespace MathApi.Models;

public class Formula
{
  public long Id { get; set; }
  public string? Meaning { get; set; }
  public List<FormulaString> FormulaStrings { get; set; } = new List<FormulaString>();
  public List<FormulaChain>? FormulaChains { get; set; }
}