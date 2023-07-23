using System.ComponentModel.DataAnnotations.Schema;

namespace MathApi.Models;

public class Formula
{
  public long Id { get; set; }
  public string? Meaning { get; set; }

  public List<FormulaString> FormulaStrings { get; set; } = new();
  public List<FormulaChain>? FormulaChains { get; set; }

  public long Length => FormulaStrings.Count;
  /// <summary>
  /// 論理式が項か命題か
  /// ※FormulaStrings.Symbol.SymbolTypeのインクルードが必要
  /// </summary>
  public long? FormulaTypeId => FormulaStrings.Count == 0 ? null : FormulaStrings[0].Symbol.SymbolType?.FormulaType.Id;

  private List<Symbol>? _FreeAndPropVariables = null;
  /// <summary>
  /// 論理式に含まれる相異なる自由・命題変数のリスト
  /// ※FormulaStringおよびFormulaString.Symbolのインクルードが必要
  /// </summary>
  [NotMapped]
  public List<Symbol> FreeAndPropVariables
  {
    get
    {
      if (_FreeAndPropVariables != null)
        return _FreeAndPropVariables;

      _FreeAndPropVariables = new List<Symbol>();
      foreach (var fs in FormulaStrings)
      {
        if (fs.Symbol.SymbolTypeId != (long)Const.SymbolType.FreeVariable)
          continue;
        if (fs.Symbol.SymbolTypeId != (long)Const.SymbolType.PropositionVariable)
          continue;
        if (_FreeAndPropVariables.Any(s => s.Id == fs.SymbolId))
          continue;
        _FreeAndPropVariables.Add(fs.Symbol);
      }
      return _FreeAndPropVariables;
    }
  }

  public List<AxiomProposition>? AxiomPropositions { get; }
  public List<TheoremConclusion>? TheoremConclusions { get; }
  public List<TheoremAssumption>? TheoremAssumptions { get; }
  public List<ProofInference>? ProofInferences { get; }
  public List<ProofInferenceArgument>? ProofInferenceArguments { get; }
  public List<ProofInferenceAssumption>? ProofInferenceAssumptions { get; }
  public List<InferenceAssumptionFormula>? InferenceAssumptionFormulas { get; }
  public List<InferenceConclusionFormula>? InferenceConclusionFormulas { get; }
}
