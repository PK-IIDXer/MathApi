namespace MathApi.Models;

using MathApi.Commons;

public class FormulaStruct
{
  public long Id { get; set; }
  public string? Meaning { get; set; }

  public Const.FormulaLabelType? TypeId
  {
    get
    {
      if (Arguments.Count == 0) 
        return null;
      return Arguments[0].Label.TypeId;
    }
  }

  public List<FormulaStructArgument> Arguments { get; set; } = new();
  public List<FormulaStructString> Strings { get; set; } = new();

  public Formula Apply(List<Formula> args)
  {
    if (args.Count != Arguments.Count)
      throw new ArgumentException("Argument mismatch");

    var tmpFormulas = new List<Formula>();
    for (var i = Strings.Count - 1; i >= 0; i--)
    {
      var fsChar = Strings[i];
      if (fsChar.ArgumentSerialNo.HasValue)
      {
        var fsArg = fsChar.Argument
          ?? throw new ArgumentException("Include FormulaStructString.Argument");
        var formula = args[fsChar.ArgumentSerialNo.Value]
          ?? throw new ArgumentException(null, nameof(args));
        if (fsArg.Label.TypeId == Const.FormulaLabelType.Term)
        {
          if (formula.FormulaTypeId != (int)Const.FormulaType.Term)
            throw new ArgumentException($"Invalid args on #{fsChar.ArgumentSerialNo.Value}");
        }
        if (fsArg.Label.TypeId == Const.FormulaLabelType.Proposition)
        {
          if (formula.FormulaTypeId != (int)Const.FormulaType.Proposition)
            throw new ArgumentException($"Invalid args on #{fsChar.ArgumentSerialNo.Value}");
        }
        if (fsArg.Label.TypeId == Const.FormulaLabelType.FreeVariable)
        {
          if (!formula.IsFreeVariable)
            throw new ArgumentException($"Invalid args on #{fsChar.ArgumentSerialNo.Value}");
        }

        foreach (var sbs in fsChar.Substitutions)
        {
          // 代入操作
          // TODO: 代入変数の重複チェック
          var fromFsArg = sbs.ArgumentFrom
            ?? throw new ArgumentException("Include FormulaStructString.SubstitutionArgumentFrom");
          if (fromFsArg.Label.TypeId != Const.FormulaLabelType.FreeVariable)
            throw new ArgumentException("Invalid FormulaStruct data");
          var fromFormula = args[sbs.ArgumentFromSerialNo]
            ?? throw new ArgumentException($"Invalid args on #{fsChar.ArgumentSerialNo.Value}");
          if (!fromFormula.IsFreeVariable)
            throw new ArgumentException($"Invalid args on #{fsChar.ArgumentSerialNo.Value}");
          var toFsArg = sbs.ArgumentTo
            ?? throw new ArgumentException("Include FormulaStructString.SubstitutionArgumentTo");
          if (toFsArg.Label.TypeId != Const.FormulaLabelType.FreeVariable)
            throw new ArgumentException("Invalid FormulaStruct data");
          var toFormula = args[sbs.ArgumentToSerialNo]
            ?? throw new ArgumentException($"Invalid args on #{fsChar.ArgumentSerialNo.Value}");
          if (!toFormula.IsFreeVariable)
            throw new ArgumentException($"Invalid args on #{fsChar.ArgumentSerialNo.Value}");

          formula = formula.Substitute(fromFormula, toFormula);
        }
        tmpFormulas.Insert(0, formula);
      }

      if (fsChar.SymbolId.HasValue)
      {
        var symbol = fsChar.Symbol
          ?? throw new ArgumentException("Include FormulaStructString.Symbol");
        if (fsChar.BoundArgument == null)
          throw new ArgumentException("Include FormulaStructString.BoundArgument");
        Formula? boundVar = null;
        if (fsChar.BoundArgumentSerialNo.HasValue)
          boundVar = args[fsChar.BoundArgumentSerialNo.Value];
        var formula = Util.ProceedFormulaConstruction(symbol, boundVar, tmpFormulas);
        tmpFormulas.Clear();
        tmpFormulas.Add(formula);
      }
    }

    return tmpFormulas[0];
  }

  public FormulaStruct Apply(List<FormulaStruct> args)
  {
    if (args.Count != Arguments.Count)
      throw new ArgumentException("Argument mismatch");

    var arguments = CreateFormulaArgument(args);
    var strings = CreateFormulaString(args, arguments);

    return new FormulaStruct
    {
      Arguments = arguments,
      Strings = strings
    };
  }

  private static List<FormulaStructArgument> CreateFormulaArgument(List<FormulaStruct> args)
  {
    var ret = new List<FormulaStructArgument>();
    var serialNo = 0;
    foreach (var arg in args)
    {
      foreach (var argArg in arg.Arguments)
      {
        if (!ret.Any(r => r.LabelId == argArg.LabelId))
        {
          ret.Add(new FormulaStructArgument
            {
              SerialNo = serialNo++,
              LabelId = argArg.LabelId
            }
          );
        }
      }
    }

    return ret;
  }

  private List<FormulaStructString> CreateFormulaString(List<FormulaStruct> args, List<FormulaStructArgument> newFormulaStructArguments)
  {
    var ret = new List<FormulaStructString>();
    var serialNo = 0;
    foreach (var str in Strings)
    {
      if (str.SymbolId.HasValue)
      {
        if (str.Symbol == null)
          throw new ArgumentException("Include FormulaStructString.String");
        if (str.BoundArgument == null)
          throw new ArgumentException("Include FormulaStructString.BoundArgument, or invalid FormulaString data");

        ret.Add(new FormulaStructString
        {
          SerialNo = serialNo++,
          Symbol = str.Symbol,
          SymbolId = str.SymbolId,
          BoundArgument = str.BoundArgument,
          BoundArgumentSerialNo = str.BoundArgumentSerialNo
        });

        if (str.Symbol.IsQuantifier)
        {
          var nextStr = Strings.Find(s => s.SerialNo == str.SerialNo + 1)
            ?? throw new ArgumentException("Invalid FormulaStructString data");
          var nextArgArg = newFormulaStructArguments.Find(fsa => fsa.LabelId == nextStr.Argument?.LabelId)
            ?? throw new ArgumentException("想定外");
          var nextArg = args[nextArgArg.SerialNo];
          if (nextArg.Strings.Any(s => s.BoundArgument?.LabelId == str.BoundArgument.LabelId))
            throw new ArgumentException("BoundArgument duplicated");
        }
      }

      if (str.ArgumentSerialNo.HasValue)
      {
        var fsArg = Arguments[str.ArgumentSerialNo ?? throw new ArgumentException("Invalid FormulaStructString data")]
          ?? throw new ArgumentException("Invalid FormulaStructString data");
        var arg = args[str.ArgumentSerialNo ?? throw new ArgumentException("Invalid FormulaStructString data")];
        if (arg.TypeId != fsArg.Label.TypeId)
          throw new ArgumentException("Argument type mismatch");
        foreach (var argStr in arg.Strings)
        {
          var argArg = newFormulaStructArguments.Find(fsa => fsa.LabelId == argStr.Argument?.LabelId)
            ?? throw new ArgumentException("想定外");

          // 代入テーブル作成
          // TODO: 代入変数の重複チェック
          var newSbs = new List<FormulaStructStringSubstitution>();
          var newSbsSerialNo = 0;
          foreach (var sbs in str.Substitutions)
          {
            foreach (var argSbs in argStr.Substitutions)
            {
              if (sbs.ArgumentFrom == null)
                throw new ArgumentException("Include FormulaStructSubstitution.ArgumentFrom");
              var newFsaFrom = newFormulaStructArguments.Find(fsa => fsa.LabelId == sbs.ArgumentFrom.LabelId)
                ?? throw new ArgumentException("想定外");
              if (sbs.ArgumentTo == null)
                throw new ArgumentException("Include FormulaStructSubstitution.ArgumentFrom");
              var newFsaTo = newFormulaStructArguments.Find(fsa => fsa.LabelId == sbs.ArgumentTo.LabelId)
                ?? throw new ArgumentException("想定外");
              if (argSbs.ArgumentFrom == null)
                throw new ArgumentException("Include FormulaStructSubstitution.ArgumentFrom");
              var argNewFsaFrom = newFormulaStructArguments.Find(fsa => fsa.LabelId == argSbs.ArgumentFrom.LabelId)
                ?? throw new ArgumentException("想定外");
              if (argSbs.ArgumentTo == null)
                throw new ArgumentException("Include FormulaStructSubstitution.ArgumentFrom");
              var argNewFsaTo = newFormulaStructArguments.Find(fsa => fsa.LabelId == argSbs.ArgumentTo.LabelId)
                ?? throw new ArgumentException("想定外");

              if (newFsaFrom.LabelId == argNewFsaTo.LabelId)
              {
                newSbs.Add(new FormulaStructStringSubstitution
                {
                  FormulaStructStringSerialNo = serialNo,
                  SerialNo = newSbsSerialNo++,
                  ArgumentFromSerialNo = argNewFsaFrom.SerialNo,
                  ArgumentToSerialNo = newFsaTo.SerialNo
                });
              }
              else if (newFsaFrom.LabelId == argNewFsaFrom.LabelId)
              {
                newSbs.Add(new FormulaStructStringSubstitution
                {
                  FormulaStructStringSerialNo = serialNo,
                  SerialNo = newSbsSerialNo++,
                  ArgumentFromSerialNo = argNewFsaFrom.SerialNo,
                  ArgumentToSerialNo = argNewFsaTo.SerialNo,
                });
              }
              else
              {
                newSbs.Add(new FormulaStructStringSubstitution
                {
                  FormulaStructStringSerialNo = serialNo,
                  SerialNo = newSbsSerialNo++,
                  ArgumentFromSerialNo = argNewFsaFrom.SerialNo,
                  ArgumentToSerialNo = argNewFsaTo.SerialNo,
                });
                newSbs.Add(new FormulaStructStringSubstitution
                {
                  FormulaStructStringSerialNo = serialNo,
                  SerialNo = newSbsSerialNo++,
                  ArgumentFromSerialNo = newFsaFrom.SerialNo,
                  ArgumentToSerialNo = newFsaTo.SerialNo,
                });
              }
            }
          }

          var newFss = new FormulaStructString
          {
            SerialNo = serialNo++,
            ArgumentSerialNo = argArg.SerialNo,
            Substitutions = newSbs
          };
          foreach (var s in newSbs)
          {
            s.FormulaStructString = newFss;
          }
          ret.Add(newFss);
        }
      }
    }

    return ret;
  }
}
