namespace MathApi.Models;

public class Inference
{
  public long Id { get; set; }
  public string Name { get; set;} = "";
  public bool IsAssumptionAdd { get; set; }
  public Theorem? Theorem { get; set; }
  public long? TheoremId { get; set; }

  public List<InferenceArgument> InferenceArguments { get; set; } = new();
  public List<InferenceAssumption> InferenceAssumptions { get; set; } = new();
  public List<InferenceConclusionFormula> InferenceConclusionFormulas { get; set; } = new();

  public List<ProofInference>? ProofInferences { get; }

  public Formula CreateConclusionFormula(List<ProofInferenceArgument> args)
  {
    if (args.Count != InferenceArguments.Count)
      throw new ArgumentException("Argument count mismatch.");

    // 論理式によって推論規則が定義されていない場合
    // (基本的な推論規則の場合)
    if (!InferenceConclusionFormulas.Any(icf => icf.FormulaId != null))
    {
      // 結論の一文字目が記号の場合
      if (InferenceConclusionFormulas[0].SymbolId != null)
      {
        var firstSymbol = InferenceConclusionFormulas[0].Symbol;
        var isQuant = firstSymbol.SymbolTypeId == (long)Const.SymbolType.TermQuantifier
                   || firstSymbol.SymbolTypeId == (long)Const.SymbolType.PropositionQuantifier;
        long? boundSymbolId;
        var argFormulas = new List<Formula>();

        for (var i = 1; i < InferenceConclusionFormulas.Count; i++)
        {
          // 束縛変数が指定されている場合
          if (InferenceConclusionFormulas[i].BoundInferenceArgumentSerialNo != null)
          {
            // 束縛変数に対応する引数に自由変数が設定されていない場合はエラー①
            if (args[InferenceConclusionFormulas[i].SerialNo].Formula.Length > 1)
              throw new ArgumentException($"too long Formula is set into bound variable position of the inference #{Id}. it may not be a free variable");

            // 束縛変数に対応する引数に自由変数が設定されていない場合はエラー②
            if (args[InferenceConclusionFormulas[i].SerialNo].Formula.FormulaStrings[0].Symbol.SymbolTypeId != (long)Const.SymbolType.FreeVariable)
              throw new ArgumentException($"Should use free variable at the position of bound variable argument of inference");

            // 束縛変数が設定されているのに、量化記号の導入でない場合はエラー
            if (!isQuant)
              throw new ArgumentException($"Should use quantifier at first if use boud variable");

            // 束縛変数のSymbolId
            boundSymbolId = args[InferenceConclusionFormulas[i].SerialNo].Formula.FormulaStrings[0].SymbolId;
          }
          else
          {
            if (InferenceConclusionFormulas[i].InferenceArgument?.InferenceArgumentTypeId != (int)Const.InferenceArgumentType.Proposition)
              throw new ArgumentException("Invalid argument");

            var targetFormula = args[InferenceConclusionFormulas[i].SerialNo].Formula;

            argFormulas.Add(args[InferenceConclusionFormulas[i].SerialNo].Formula);
          }
        }
      }
    }
  }

  /// <summary>
  /// POSTパラメータからFormulaStringリストを作成する
  /// </summary>
  /// <param name="firstSymbol">一文字目</param>
  /// <param name="argFormulaIds">POSTで渡された論理式IDのリスト</param>
  /// <param name="argFormulas">引数論理式のオブジェクトリスト</param>
  /// <param name="isQuant">一文字目が量化記号かどうか</param>
  /// <param name="boundVariable">束縛変数文字オブジェクト</param>
  /// <param name="boundId">束縛する自由変数のFormulaID</param>
  /// <returns>FormulaStringのリスト</returns>
  /// <exception cref="InvalidDataException">
  /// 量化記号を使うのに束縛変数記号がDBに登録されてないときにThrow
  /// </exception>
  private static List<FormulaString> CreateFormulaStringFromPostParam(
    Symbol firstSymbol,
    List<long> argFormulaIds,
    List<Formula> argFormulas,
    bool isQuant,
    Symbol? boundVariable,
    long? boundId
  )
  {
    long serialNo = 0;

    // 一文字目をセット
    var strings = new List<FormulaString>
    {
      new FormulaString
      {
        SerialNo = serialNo++,
        SymbolId = firstSymbol.Id,
        Symbol = firstSymbol
      }
    };

    // 引数の論理式を一文字ずつバラシてstringsに追加する
    foreach (var argFormulaId in argFormulaIds)
    {
      var argFormulaItem = argFormulas.First(f => f.Id == argFormulaId);
      var argFormulaStrings = argFormulaItem.FormulaStrings;
      foreach (var argFormulaString in argFormulaStrings)
      {
        // 量化する場合、対象の自由変数を束縛変数に変換する。
        if (isQuant && argFormulaString.SymbolId == boundId)
        {
          if (boundVariable == null)
          {
            throw new InvalidDataException("Not Found bound variable. Register bound variable symbol");
          }
          strings.Add(
            new FormulaString
            {
              SerialNo = serialNo++,
              SymbolId = boundVariable.Id
            }
          );
        }
        // 量化しない場合
        else
        {
          strings.Add(
            new FormulaString
            {
              SerialNo = serialNo++,
              SymbolId = argFormulaString.SymbolId
            }
          );
        }
      }
    }
    return strings;
  }
}
