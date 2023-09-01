using MathApi.Models;

namespace MathApi.Controllers;

public class SymbolDto
{
  public long Id { get; set; }
  public string Character { get; set; } = "";
  public Const.SymbolType SymbolTypeId { get; set; }
  public int? Arity { get; set; }
  public Const.FormulaType? ArityFormulaTypeId { get; set; }
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

  private static int? FindArity(Const.SymbolType symbolTypeId, int? arity)
  {
    if (symbolTypeId == Const.SymbolType.FreeVariable)
    {
      if (arity.HasValue)
        throw new ArgumentException("can't set Arity since it is always null if FreeVariable");
      return null;
    }
    if (symbolTypeId == Const.SymbolType.BoundVariable)
    {
      if (arity.HasValue)
        throw new ArgumentException("can't set Arity since it is always null if BoundVariable");
      return null;
    }
    if (symbolTypeId == Const.SymbolType.Function)
    {
      if (!arity.HasValue)
        throw new ArgumentException("Set Arity if Function");
      return arity;
    }
    if (symbolTypeId == Const.SymbolType.Predicate)
    {
      if (!arity.HasValue)
        throw new ArgumentException("Set Arity if Predicate");
      return arity;
    }
    if (symbolTypeId == Const.SymbolType.Logic)
    {
      if (!arity.HasValue)
        throw new ArgumentException("Set Arity if Logic");
      return arity;
    }
    if (symbolTypeId == Const.SymbolType.TermQuantifier)
    {
      if (arity.HasValue)
        throw new ArgumentException("can't set Arity since it is always 1 if TermQuantifier");
      return 1;
    }
    if (symbolTypeId == Const.SymbolType.PropositionQuantifier)
    {
      if (arity.HasValue)
        throw new ArgumentException("can't set Arity since it is always 1 if PropositionQuantifier");
      return 1;
    }
    throw new NotImplementedException();
  }

  private static Const.FormulaType? FindArityFormulaTypeId(Const.SymbolType symbolTypeId, Const.FormulaType? arityFormulaTypeId)
  {
    if (symbolTypeId == Const.SymbolType.FreeVariable)
    {
      if (arityFormulaTypeId.HasValue)
        throw new ArgumentException("can't set ArityFormulaTypeId since it is always null if FreeVariable");
      return null;
    }
    if (symbolTypeId == Const.SymbolType.BoundVariable)
    {
      if (arityFormulaTypeId.HasValue)
        throw new ArgumentException("can't set ArityFormulaTypeId since it is always null if BoundVariable");
      return null;
    }
    if (symbolTypeId == Const.SymbolType.Function)
    {
      if (arityFormulaTypeId.HasValue)
        throw new ArgumentException("can't set ArityFormulaTypeId since it is always Term if Function");
      return Const.FormulaType.Term;
    }
    if (symbolTypeId == Const.SymbolType.Predicate)
    {
      if (arityFormulaTypeId.HasValue)
        throw new ArgumentException("can't set ArityFormulaTypeId since it is always Term if Predicate");
      return Const.FormulaType.Term;
    }
    if (symbolTypeId == Const.SymbolType.Logic)
    {
      if (arityFormulaTypeId.HasValue)
        throw new ArgumentException("can't set ArityFormulaTypeId since it is always Proposition if Logic");
      return Const.FormulaType.Proposition;
    }
    if (symbolTypeId == Const.SymbolType.TermQuantifier)
    {
      if (!arityFormulaTypeId.HasValue)
        throw new ArgumentException("Set ArityFormulaTypeId if TermQuantifier");
      return arityFormulaTypeId;
    }
    if (symbolTypeId == Const.SymbolType.PropositionQuantifier)
    {
      if (!arityFormulaTypeId.HasValue)
        throw new ArgumentException("Set ArityFormulaTypeId if PropositionQuantifier");
      return arityFormulaTypeId;
    }
    throw new NotImplementedException();
  }
}
