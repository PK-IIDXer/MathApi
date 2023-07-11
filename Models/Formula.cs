namespace MathApi.Models;

public class Formula
{
  public long Id { get; set; }
  public string? Meaning { get; set; }

  public List<FormulaString> FormulaStrings { get; } = new();
  public List<FormulaChain>? FormulaChains { get; }

  public long Length => FormulaStrings.Count;
  public long? FormulaTypeId => FormulaStrings.Count == 0 ? null : FormulaStrings[0].Symbol.SymbolType?.FormulaType.Id;

  public List<TheoremConclusion>? TheoremConclusions { get; }
  public List<TheoremAssumption>? TheoremAssumptions { get; }
  public List<Proof>? Proofs { get; }
  public List<ProofArgument>? ProofArguments { get; }
  public List<ProofAssumption>? ProofAssumptions { get; }
}
