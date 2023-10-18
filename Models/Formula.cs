using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MathApi.Models;

public class Formula
{
  public long Id { get; set; }
  public string? Meaning { get; set; }

  public List<FormulaString> Strings { get; set; } = new();
  public List<FormulaChain> Chains { get; set; } = new();

  public long Length => Strings.Count;
  /// <summary>
  /// 論理式が項か命題か
  /// ※FormulaStrings.Symbol.SymbolTypeのインクルードが必要
  /// </summary>
  [JsonIgnore]
  public Const.FormulaTypeEnum? FormulaTypeId
  {
    get
    {
      if (Strings[0].Symbol == null)
        throw new ArgumentException("Include FormulaStrings.Symbol");
      if (Strings[0].Symbol?.Type == null)
        throw new ArgumentException("Include FormulaStrings.Symbol.Type");
      return Strings.Count == 0 ? null : Strings[0].Symbol?.Type?.FormulaTypeId;
    }
  }

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
      foreach (var fs in Strings)
      {
        if (fs.Symbol == null)
          throw new ArgumentException("Include FormulaString.Symbol");
        if (fs.Symbol.TypeId != Const.SymbolTypeEnum.FreeVariable)
          continue;
        if (_FreeAndPropVariables.Any(s => s.Id == fs.SymbolId))
          continue;
        _FreeAndPropVariables.Add(fs.Symbol);
      }
      return _FreeAndPropVariables;
    }
  }

  /// <summary>
  /// 自由変数かどうか
  /// ※以下のインクルードが必要
  /// ・FormulaStrings
  /// ・FormulaChains
  /// </summary>
  public bool IsFreeVariable
  {
    get
    {
      if (Strings.Count != 1)
        return false;
      if (Chains.Count != 0)
        return false;
      if (Strings[0].Symbol == null)
        throw new ArgumentException("Include FormulaStrings.Symbol");
      return Strings[0].Symbol?.TypeId == Const.SymbolTypeEnum.FreeVariable;
    }
  }

  public List<AxiomProposition>? AxiomPropositions { get; }
  public List<TheoremConclusion>? TheoremConclusions { get; }
  public List<TheoremAssumption>? TheoremAssumptions { get; }
  public List<ProofInference>? ProofInferences { get; }
  public List<ProofInferenceArgument>? ProofInferenceArguments { get; }

  /// <summary>
  /// 変数への代入操作
  /// ※以下のインクルードが必要
  /// ・FormulaStrings
  /// ・FormulaStrings.Symbol
  /// ・from.FormulaStrings
  /// ・from.FormulaStrings.Symbol
  /// ・from.FormulaChains
  /// ・to.FormulaStrings
  /// </summary>
  /// <param name="from">代入先変数（自由または命題変数）</param>
  /// <param name="to">代入する論理式</param>
  /// <returns></returns>
  /// <exception cref="ArgumentException"></exception>
  public Formula Substitute(Formula from, Formula to)
  {
    if (!from.IsFreeVariable)
      throw new ArgumentException("from formula should be free variable");

    var fromSymbol = from.Strings[0].Symbol
      ?? throw new ArgumentException("Include FormulaStrings.Symbol");

    if (!FreeAndPropVariables.Any(s => s.Id == fromSymbol.Id))
      return this;
    
    if (fromSymbol.TypeId == Const.SymbolTypeEnum.FreeVariable)
    {
      if (to.FormulaTypeId != Const.FormulaTypeEnum.Term)
        throw new ArgumentException("substitution destination formula should be term if substitution source symbol is free variable");
    }

    var fs = CreateFormulaStringOnSubstitution(fromSymbol, to);
    var fc = CreateFormulaChainOnSubstitution(fromSymbol, to);

    return new Formula
    {
      Meaning = $"created by substituting formula#{to.Id} for symbol#{fromSymbol.Id} in formula#{Id}",
      Strings = fs,
      Chains = fc
    };
  }

  /// <summary>
  /// ※以下のインクルードが必要
  /// ・FormulaStrings
  /// ・to.FormulaStrings
  /// </summary>
  private List<FormulaString> CreateFormulaStringOnSubstitution(Symbol from, Formula to)
  {
    var fs = new List<FormulaString>();
    foreach (var s in Strings)
    {
      if (s.SymbolId != from.Id)
      {
        fs.Add(new FormulaString
        {
          SymbolId = s.SymbolId
        });
        continue;
      }

      fs.AddRange(to.Strings.Select(e => new FormulaString
      {
        SymbolId = e.SymbolId
      }).ToList());
    }

    for (var i = 0; i < fs.Count; i++)
    {
      fs[i].SerialNo = i;
    }

    return fs;
  }

  /// <summary>
  /// ※以下のインクルードが必要
  /// ・FormulaStrings
  /// ・FormulaChains
  /// ・to.FormulaStrings
  /// </summary>
  private List<FormulaChain> CreateFormulaChainOnSubstitution(Symbol from, Formula to)
  {
    var formulaLength = to.Length;
    var fc = Chains.Select(e => new FormulaChain
    {
      FromFormulaStringSerialNo = e.FromFormulaStringSerialNo,
      ToFormulaStringSerialNo = e.ToFormulaStringSerialNo
    }).ToList();

    var replaceSymbolCount = 0;
    foreach (var str in Strings.Where(e => e.SymbolId == from.Id))
    {
      replaceSymbolCount++;

      // 置き換える文字を跨いで束縛しあっている鎖がある場合
      var betweenFc = fc.Where(
        e => e.FromFormulaStringSerialNo < str.SerialNo
          && str.SerialNo < e.ToFormulaStringSerialNo
      );
      foreach (var ch in betweenFc)
      {
        fc.Add(new FormulaChain
        {
          FromFormulaStringSerialNo = ch.FromFormulaStringSerialNo,
          ToFormulaStringSerialNo = ch.ToFormulaStringSerialNo + formulaLength
        });
      }

      // 置き換える文字の後に鎖がある場合
      var runawayFc = fc.Where(
        e => e.FromFormulaStringSerialNo > str.SerialNo
      );
      foreach (var ch in runawayFc)
      {
        fc.Add(new FormulaChain
        {
          FromFormulaStringSerialNo = ch.FromFormulaStringSerialNo + formulaLength,
          ToFormulaStringSerialNo = ch.ToFormulaStringSerialNo + formulaLength
        });
      }

      // 置き換えた文字列の鎖の追加
      var replaceSymbolIdx = Strings.IndexOf(str);
      var serialNoDiff = replaceSymbolIdx + (replaceSymbolCount - 1) * formulaLength;
      to.Chains.ForEach(ch => {
        fc.Add(new FormulaChain
        {
          FromFormulaStringSerialNo = ch.FromFormulaStringSerialNo + serialNoDiff,
          ToFormulaStringSerialNo = ch.ToFormulaStringSerialNo + serialNoDiff
        });
      });
    }

    for (var i = 0; i < fc.Count; i++)
    {
      fc[i].SerialNo = i;
    }

    return fc;
  }

  /// <summary>
  /// ※以下のインクルードが必要
  /// ・FormulaStrings
  /// ・FormulaChains
  /// ・target.FormulaStrings
  /// ・target.FormulaChains
  /// </summary>
  public bool Equals(Formula target)
  {
    if (Id == target.Id)
      return true;

    if (Strings.Count != target.Strings.Count)
      return false;

    if (Chains.Count != target.Chains.Count)
      return false;

    for (var i = 0; i < Strings.Count; i++)
    {
      if (Strings[i].SymbolId != target.Strings[i].SymbolId)
        return false;
    }

    var sortedChains = Chains.OrderBy(c => c.FromFormulaStringSerialNo)
                                    .ThenBy(c => c.ToFormulaStringSerialNo)
                                    .ToList();
    var sortedTarget = target.Chains
                             .OrderBy(c => c.FromFormulaStringSerialNo)
                             .ThenBy(c => c.ToFormulaStringSerialNo)
                             .ToList();
    for (var i = 0; i < Chains.Count; i++)
    {
      if (sortedChains[i].FromFormulaStringSerialNo != sortedTarget[i].FromFormulaStringSerialNo
        || sortedChains[i].ToFormulaStringSerialNo != sortedTarget[i].ToFormulaStringSerialNo)
        return false;
    }

    return true;
  }
}
