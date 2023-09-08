using MathApi.Models;

namespace MathApi.Controllers;

public class SymbolDto
{
  public long Id { get; set; }
  public string Character { get; set; } = "";
  public Const.SymbolTypeEnum SymbolTypeId { get; set; }
  public int? Arity { get; set; }
  public Const.FormulaTypeEnum? ArityFormulaTypeId { get; set; }
  public string? Meaning { get; set; }

  public Symbol CreateModel()
  {
    return new Symbol
    {
      Id = Id,
      Character = Character,
      TypeId = SymbolTypeId,
      Arity = FindArity(SymbolTypeId, Arity),
      ArityFormulaTypeId = FindArityFormulaTypeId(SymbolTypeId, ArityFormulaTypeId),
      Meaning = Meaning
    };
  }

  private static int? FindArity(Const.SymbolTypeEnum symbolTypeId, int? arity)
  {
    if (symbolTypeId == Const.SymbolTypeEnum.FreeVariable)
    {
      if (arity.HasValue)
        throw new ArgumentException("can't set Arity since it is always null if FreeVariable");
      return null;
    }
    if (symbolTypeId == Const.SymbolTypeEnum.BoundVariable)
    {
      if (arity.HasValue)
        throw new ArgumentException("can't set Arity since it is always null if BoundVariable");
      return null;
    }
    if (symbolTypeId == Const.SymbolTypeEnum.Function)
    {
      if (!arity.HasValue)
        throw new ArgumentException("Set Arity if Function");
      return arity;
    }
    if (symbolTypeId == Const.SymbolTypeEnum.Predicate)
    {
      if (!arity.HasValue)
        throw new ArgumentException("Set Arity if Predicate");
      return arity;
    }
    if (symbolTypeId == Const.SymbolTypeEnum.Logic)
    {
      if (!arity.HasValue)
        throw new ArgumentException("Set Arity if Logic");
      return arity;
    }
    if (symbolTypeId == Const.SymbolTypeEnum.TermQuantifier)
    {
      if (arity.HasValue)
        throw new ArgumentException("can't set Arity since it is always 1 if TermQuantifier");
      return 1;
    }
    if (symbolTypeId == Const.SymbolTypeEnum.PropositionQuantifier)
    {
      if (arity.HasValue)
        throw new ArgumentException("can't set Arity since it is always 1 if PropositionQuantifier");
      return 1;
    }
    throw new NotImplementedException();
  }

  private static Const.FormulaTypeEnum? FindArityFormulaTypeId(Const.SymbolTypeEnum symbolTypeId, Const.FormulaTypeEnum? arityFormulaTypeId)
  {
    if (symbolTypeId == Const.SymbolTypeEnum.FreeVariable)
    {
      if (arityFormulaTypeId.HasValue)
        throw new ArgumentException("can't set ArityFormulaTypeId since it is always null if FreeVariable");
      return null;
    }
    if (symbolTypeId == Const.SymbolTypeEnum.BoundVariable)
    {
      if (arityFormulaTypeId.HasValue)
        throw new ArgumentException("can't set ArityFormulaTypeId since it is always null if BoundVariable");
      return null;
    }
    if (symbolTypeId == Const.SymbolTypeEnum.Function)
    {
      if (arityFormulaTypeId.HasValue)
        throw new ArgumentException("can't set ArityFormulaTypeId since it is always Term if Function");
      return Const.FormulaTypeEnum.Term;
    }
    if (symbolTypeId == Const.SymbolTypeEnum.Predicate)
    {
      if (arityFormulaTypeId.HasValue)
        throw new ArgumentException("can't set ArityFormulaTypeId since it is always Term if Predicate");
      return Const.FormulaTypeEnum.Term;
    }
    if (symbolTypeId == Const.SymbolTypeEnum.Logic)
    {
      if (arityFormulaTypeId.HasValue)
        throw new ArgumentException("can't set ArityFormulaTypeId since it is always Proposition if Logic");
      return Const.FormulaTypeEnum.Proposition;
    }
    if (symbolTypeId == Const.SymbolTypeEnum.TermQuantifier)
    {
      if (!arityFormulaTypeId.HasValue)
        throw new ArgumentException("Set ArityFormulaTypeId if TermQuantifier");
      return arityFormulaTypeId;
    }
    if (symbolTypeId == Const.SymbolTypeEnum.PropositionQuantifier)
    {
      if (!arityFormulaTypeId.HasValue)
        throw new ArgumentException("Set ArityFormulaTypeId if PropositionQuantifier");
      return arityFormulaTypeId;
    }
    throw new NotImplementedException();
  }
}
