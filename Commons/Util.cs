namespace MathApi.Commons;

using MathApi.Const;
using MathApi.Models;

public static class Util
{
  /// <summary>
  /// 論理式の構成手続き
  /// ※以下のインクルードが必要
  /// ・arguments.FormulaStrings
  /// ・arguments.FormulaChains
  /// ・boundVariable.FormulaStrings
  /// </summary>
  /// <param name="symbol">一文字目の記号</param>
  /// <param name="boundVariable">束縛先の自由変数を表す論理式オブジェクト(symbolが量化記号の場合)</param>
  /// <param name="arguments">symbolに渡す引数</param>
  /// <returns>新しいFormulaオブジェクト</returns>
  public static Formula ProceedFormulaConstruction(Symbol? symbol, Formula? boundVariable, List<Formula> arguments)
  {
    if (symbol == null)
    {
      if (arguments.Count != 1)
        throw new ArgumentException("\"argument\" should be 1-element if symbol is null");

      return arguments[0];
    }

    if (symbol.IsQuantifier)
    {
      if (boundVariable == null)
        throw new ArgumentNullException(nameof(boundVariable), "must boundVariable be not null if symbol is quantifier");
    }

    if (boundVariable != null)
    {
      if (!boundVariable.IsFreeVariable)
        throw new ArgumentException("cannot set boundVariable that is not free variable", nameof(boundVariable));
    }

    if (symbol.Arity != arguments.Count)
      throw new ArgumentException("arguments count is not match");

    var fs = new List<FormulaString>
    {
      new FormulaString
      {
        SymbolId = symbol.Id
      }
    };
    var fc = new List<FormulaChain>();
    for (var i = 0; i < arguments.Count; i++)
    {
      if (symbol.ArityFormulaTypeId != arguments[i].FormulaTypeId)
        throw new ArgumentException($"argument of {i}-th is not match to symbol.ArityFormulaType");

      // FormulaChainsの作成
      foreach (var c in arguments[i].FormulaChains)
      {
        fc.Add(new FormulaChain
        {
          FromFormulaStringSerialNo = fs.Count + c.FromFormulaStringSerialNo,
          ToFormulaStringSerialNo = fs.Count + c.ToFormulaStringSerialNo
        });
      }

      // FormulaStringsの作成
      foreach (var s in arguments[i].FormulaStrings.OrderBy(e => e.SerialNo))
      {
        var symbolId = s.SymbolId;
        // boudVariableが設定されているとき、ループ中の文字がboudVariableの場合、
        // その文字を□に置き換えて、先頭の文字(量化記号)と鎖で結ぶ
        if (boundVariable?.FormulaStrings[0].SymbolId == s.SymbolId)
        {
          symbolId = (long)BasicSymbol.BoundVariable;
          fc.Add(new FormulaChain
          {
            FromFormulaStringSerialNo = 0,
            ToFormulaStringSerialNo = fs.Count
          });
        }

        fs.Add(new FormulaString
        {
          SymbolId = symbolId
        });
      }
    }

    // FormulaStrings、FormulaChainsのSerialNoのセット
    for (var i = 0; i < fs.Count; i++)
    {
      fs[i].SerialNo = i;
    }
    for (var i = 0; i < fc.Count; i++)
    {
      fc[i].SerialNo = i;
    }

    // Formula.Meaningの作成
    var meanings = $"Created by symbol #{symbol.Id} applying formulas #"
      + string.Join(", #", arguments.Select(e => e.Id.ToString()));
    if (boundVariable != null)
      meanings += $"(bound variable symbolId: #{boundVariable.FormulaStrings[0].SymbolId})";
    meanings += ".";

    return new Formula
    {
      Meaning = meanings,
      FormulaStrings = fs,
      FormulaChains = fc
    };
  }

  /// <summary>
  /// ProofInferenceFormulaを作成
  /// </summary>
  /// <param name="inferenceFormulas">推論規則データ</param>
  /// <param name="proofInferenceArguments">引数</param>
  /// <returns>Formula</returns>
  /// <exception cref="ArgumentException">データ不正、インクルード不足</exception>
  public static Formula CreateProofInferenceFormula(List<IInferenceFormula> inferenceFormulas, List<ProofInferenceArgument> proofInferenceArguments)
  {
    var tmpFormulas = new List<Formula>();
    for (var i = inferenceFormulas.Count - 1; i >= 0; i--)
    {
      var iaf = inferenceFormulas[i];
      if (iaf.InferenceArgumentSerialNo.HasValue)
      {
        var pia = proofInferenceArguments.Find(pia => pia.SerialNo == iaf.InferenceArgumentSerialNo)
          ?? throw new ArgumentException("SerialNos of InferenceAssumptionFormulas and ProofInferenceArguments are mismatch");
        if (pia.Formula == null)
          throw new ArgumentException("Include ProofInferenceArgument.Formula");

        if (iaf.SubstitutionInferenceArgumentFromSerialNo.HasValue
          || iaf.SubstitutionInferenceArgumentToSerialNo.HasValue)
        {
          // 代入操作を行う場合
          var fromPia = proofInferenceArguments.Find(pia => pia.SerialNo == iaf.SubstitutionInferenceArgumentFromSerialNo)
            ?? throw new ArgumentException("SerialNos of InferenceAssumptionFormulas and ProofInferenceArguments are mismatch");
          var toPia = proofInferenceArguments.Find(pia => pia.SerialNo == iaf.SubstitutionInferenceArgumentToSerialNo)
            ?? throw new ArgumentException("SerialNos of InferenceAssumptionFormulas and ProofInferenceArguments are mismatch");
          var from = fromPia.Formula ?? throw new ArgumentException("Include ProofInferenceArgument.Formula");
          var to = toPia.Formula ?? throw new ArgumentException("Include ProofInferenceArgument.Formula");

          tmpFormulas.Insert(0, pia.Formula.Substitute(from, to));
        }
        else
        {
          // 代入操作を行わない場合
          tmpFormulas.Insert(0, pia.Formula);
        }
      }

      if (iaf.SymbolId.HasValue)
      {
        var symbol = iaf.Symbol ?? throw new ArgumentException("Include InferenceArgumentFormula.Symbol");
        var boundVarPia = proofInferenceArguments.Find(pia => pia.SerialNo == iaf.BoundInferenceArgumentSerialNo);
        var formula = ProceedFormulaConstruction(symbol, boundVarPia?.Formula, tmpFormulas);
        tmpFormulas.Clear();
        tmpFormulas.Add(formula);
      }
    }

    return tmpFormulas[0];
  }
}