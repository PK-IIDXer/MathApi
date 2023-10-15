namespace MathApi.Models;

using MathApi.Commons;
using System.Text.Json.Serialization;

public class FormulaStruct
{
  public long Id { get; set; }
  public string? Meaning { get; set; }

  [JsonIgnore]
  public Const.FormulaLabelTypeEnum TypeId
  {
    get
    {
      var s = Strings[0]
        ?? throw new ArgumentException("Include FormulaStruct.Strings");

      if (s.SymbolId.HasValue)
      {
        if (s.Symbol == null)
          throw new ArgumentException("Include FormulaStruct.Strings.Symbol");

        return
          s.Symbol.TypeId
        switch
        {
            Const.SymbolTypeEnum.FreeVariable
              => Const.FormulaLabelTypeEnum.FreeVariable,
            Const.SymbolTypeEnum.Function
              or Const.SymbolTypeEnum.TermQuantifier
              => Const.FormulaLabelTypeEnum.Term,
            Const.SymbolTypeEnum.Predicate
              or Const.SymbolTypeEnum.Logic
              or Const.SymbolTypeEnum.PropositionQuantifier
              => Const.FormulaLabelTypeEnum.Proposition,
            _ => throw new ArgumentException("想定外"),
        };
      }

      if (s.ArgumentSerialNo.HasValue)
      {
        if (s.Argument == null)
          throw new ArgumentException("Include FormulaStruct.Strings.Argument");
        if (s.Argument.Label == null)
          throw new ArgumentException("Include FormulaStruct.Strings.Argument.Label");
        return s.Argument.Label.TypeId;
      }

      throw new ArgumentException("想定外");
    }
  }

  public List<FormulaStructArgument> Arguments { get; set; } = new();
  public List<FormulaStructString> Strings { get; set; } = new();

  [JsonIgnore]
  public List<InferenceAssumption> InferenceAssumptions { get; } = new();
  [JsonIgnore]
  public List<InferenceAssumptionDissolutableAssumption> InferenceAssumptionDissolutableAssumptions { get; } = new();
  [JsonIgnore]
  public List<InferenceConclusion> InferenceConclusions { get; } = new();
  [JsonIgnore]
  public List<ProofInference> ProofInferences { get; } = new();
  [JsonIgnore]
  public List<ProofInferenceArgument> ProofInferenceArguments { get; } = new();

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
        if (fsArg.Label == null)
          throw new ArgumentException("Include FormulaStruct.Strings.Argument.Label");
        if (fsArg.Label.TypeId == Const.FormulaLabelTypeEnum.Term)
        {
          if (formula.FormulaTypeId != Const.FormulaTypeEnum.Term)
            throw new ArgumentException($"Invalid args on #{fsChar.ArgumentSerialNo.Value}");
        }
        if (fsArg.Label.TypeId == Const.FormulaLabelTypeEnum.Proposition)
        {
          if (formula.FormulaTypeId != Const.FormulaTypeEnum.Proposition)
            throw new ArgumentException($"Invalid args on #{fsChar.ArgumentSerialNo.Value}");
        }
        if (fsArg.Label.TypeId == Const.FormulaLabelTypeEnum.FreeVariable)
        {
          if (!formula.IsFreeVariable)
            throw new ArgumentException($"Invalid args on #{fsChar.ArgumentSerialNo.Value}");
        }

        var fromVars = new List<Formula>();
        foreach (var sbs in fsChar.Substitutions)
        {
          // 代入操作
          var fromFsArg = sbs.ArgumentFrom
            ?? throw new ArgumentException("Include FormulaStructString.SubstitutionArgumentFrom");
          if (fromFsArg.Label == null)
            throw new ArgumentException("Include FormulaStruct.Strings.Argument.Label");
          if (fromFsArg.Label.TypeId != Const.FormulaLabelTypeEnum.FreeVariable)
            throw new ArgumentException("Invalid FormulaStruct data");
          var fromFormula = args[sbs.ArgumentFromSerialNo]
            ?? throw new ArgumentException($"Invalid args on #{fsChar.ArgumentSerialNo.Value}");
          if (!fromFormula.IsFreeVariable)
            throw new ArgumentException($"Invalid args on #{fsChar.ArgumentSerialNo.Value}");
          var toFsArg = sbs.ArgumentTo
            ?? throw new ArgumentException("Include FormulaStructString.SubstitutionArgumentTo");
          if (toFsArg.Label == null)
            throw new ArgumentException("Include FormulaStruct.Strings.Argument.Label");
          if (toFsArg.Label.TypeId != Const.FormulaLabelTypeEnum.FreeVariable)
            throw new ArgumentException("Invalid FormulaStruct data");
          var toFormula = args[sbs.ArgumentToSerialNo]
            ?? throw new ArgumentException($"Invalid args on #{fsChar.ArgumentSerialNo.Value}");
          if (!toFormula.IsFreeVariable)
            throw new ArgumentException($"Invalid args on #{fsChar.ArgumentSerialNo.Value}");
          // 代入元変数重複チェック
          if (fromVars.Any(f => f.Equals(fromFormula)))
            throw new ArgumentException("from variable duplicate");
          else
            fromVars.Add(fromFormula);

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
        var arity = symbol.Arity ?? 0;
        var symbolArgFormulas = tmpFormulas.Take(0..arity).ToList();
        tmpFormulas.RemoveRange(0, arity);
        var formula = Util.ProceedFormulaConstruction(symbol, boundVar, symbolArgFormulas);
        tmpFormulas.Add(formula);
      }
    }

    if (tmpFormulas.Count > 1)
      throw new ArgumentException("Fail to construct a formula");
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
        if (fsArg.Label == null)
          throw new ArgumentException("Include FormulaStruct.Arguments.Label");
        if (arg.TypeId != fsArg.Label.TypeId)
          throw new ArgumentException("Argument type mismatch");
        foreach (var argStr in arg.Strings)
        {
          var argArg = newFormulaStructArguments.Find(fsa => fsa.LabelId == argStr.Argument?.LabelId)
            ?? throw new ArgumentException("想定外");

          // 代入テーブル作成
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
          var froms = new List<int>();
          foreach (var s in newSbs)
          {
            if (s.ArgumentFrom != null && s.ArgumentFrom.Label == null)
              throw new ArgumentException("Include FormulaStructStringSubstitution.ArgumentFrom.Label");
            if (froms.Any(f => f == s.ArgumentFrom?.Label?.Id))
              throw new ArgumentException("Substitution \"from\" variable is duplicated");
            else
              froms.Add(s.ArgumentFromSerialNo);
            s.FormulaStructString = newFss;
          }
          ret.Add(newFss);
        }
      }
    }

    return ret;
  }

  public bool Equals(FormulaStruct formulaStruct)
  {
    if (Strings.Count != formulaStruct.Strings.Count)
      return false;
    for (int i = 0; i < Strings.Count; i++)
    {
      var str = Strings.Find(a => a.SerialNo == i) ?? throw new ArgumentException("想定外");
      var fsStr = formulaStruct.Strings.Find(a => a.SerialNo == i) ?? throw new ArgumentException("想定外");

      if (str.SymbolId != fsStr.SymbolId)
        return false;

      if (str.BoundArgument?.LabelId != fsStr.BoundArgument?.LabelId)
        return false;

      if (str.Argument?.LabelId != fsStr.Argument?.LabelId)
        return false;

      if (str.Substitutions.Count != fsStr.Substitutions.Count)
        return false;

      foreach (var sb in str.Substitutions)
      {
        if (sb.ArgumentFrom == null)
          throw new ArgumentException("Include FormulaStruct.Substitutions.ArgumentFrom");
        if (sb.ArgumentTo == null)
          throw new ArgumentException("Include FormulaStruct.Substitutions.ArgumentTo");
        var fsSb = fsStr.Substitutions.FirstOrDefault(s =>
        {
          if (s.ArgumentFrom == null)
            throw new ArgumentException("Include FormulaStruct.Substitutions.ArgumentFrom");
          if (s.ArgumentTo == null)
            throw new ArgumentException("Include FormulaStruct.Substitutions.ArgumentFrom");
          return s.ArgumentFrom.LabelId == sb.ArgumentFrom.LabelId
            && s.ArgumentTo.LabelId == sb.ArgumentTo.LabelId;
        });
        if (fsSb == null)
          return false;
      }
      foreach (var fsSb in fsStr.Substitutions)
      {
        if (fsSb.ArgumentFrom == null)
          throw new ArgumentException("Include FormulaStruct.Substitutions.ArgumentFrom");
        if (fsSb.ArgumentTo == null)
          throw new ArgumentException("Include FormulaStruct.Substitutions.ArgumentTo");
        var sb = fsStr.Substitutions.FirstOrDefault(s =>
        {
          if (s.ArgumentFrom == null)
            throw new ArgumentException("Include FormulaStruct.Substitutions.ArgumentFrom");
          if (s.ArgumentTo == null)
            throw new ArgumentException("Include FormulaStruct.Substitutions.ArgumentFrom");
          return s.ArgumentFrom.LabelId == fsSb.ArgumentFrom.LabelId
            && s.ArgumentTo.LabelId == fsSb.ArgumentTo.LabelId;
        });
        if (sb == null)
          return false;
      }
    }

    return true;
  }
}
