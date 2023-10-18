using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MathApi.Models;
using MathApi.Commons;

namespace MathApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FormulaController : ControllerBase
  {
    private readonly MathDbContext _context;

    public FormulaController(MathDbContext context)
    {
      _context = context;
    }

    // GET: api/Formula
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Formula>>> GetFormulas([FromQuery] string? meaning)
    {
      return await _context
        .Formulas
        .IgnoreAutoIncludes()
        .Include(f => f.Strings)
        .ThenInclude(fs => fs.Symbol)
        .Include(f => f.Chains)
        .Where(f => string.IsNullOrEmpty(meaning) || meaning.Contains(f.Meaning ?? ""))
        .ToListAsync();
    }

    // GET: api/Formula/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Formula>> GetFormula(long id)
    {
      var formula = await _context.Formulas.FindAsync(id);

      if (formula == null)
      {
        return NotFound();
      }

      return formula;
    }

    // PUT: api/Formula/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutFormula(long id, Formula formula)
    {
      // TODO: 実装

      if (id != formula.Id)
      {
        return BadRequest();
      }

      _context.Entry(formula).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!FormulaExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/Formula
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Formula>> PostFormula(FormulaDto dto)
    {
      try
      {
        var formula = dto.CreateModel();
        var symbols = await _context.Symbols.Where(s => formula.Strings.Select(st => st.SymbolId).Contains(s.Id)).ToListAsync();

        // FormulaStringチェック
        var tmpSymbols = new List<Symbol>();
        for (var i = formula.Strings.Count - 1; i >= 0; i--)
        {
          var str = formula.Strings[i];
          var symbol = symbols.Find(s => s.Id == str.SymbolId);
          if (symbol == null)
            return BadRequest($"Symbol#{str.SymbolId} is not found");

          if (!symbol.Arity.HasValue || symbol.Arity == 0)
          {
            tmpSymbols.Insert(0, symbol);
          }
          else
          {
            if (symbol.Arity > tmpSymbols.Count)
              return BadRequest($"argument count of symbol#{symbol.Id} (StringsSerialNo={str.SerialNo}) is invalid");

            for (var j = 0; j < symbol.Arity; j++)
            {
              var tmpSymbol = tmpSymbols[j];
              if (tmpSymbol.Type == null)
              {
                if (symbol.ArityFormulaTypeId == Const.FormulaTypeEnum.Term)
                {
                  switch (tmpSymbol.TypeId)
                  {
                    case Const.SymbolTypeEnum.FreeVariable:
                    case Const.SymbolTypeEnum.Function:
                    case Const.SymbolTypeEnum.TermQuantifier:
                      continue;
                    case Const.SymbolTypeEnum.Logic:
                    case Const.SymbolTypeEnum.Predicate:
                    case Const.SymbolTypeEnum.PropositionQuantifier:
                      return BadRequest($"ArityType mismatch (StringsSerialNo={str.SerialNo})");
                    default:
                      throw new NotImplementedException();
                  }
                }

                if (symbol.ArityFormulaTypeId == Const.FormulaTypeEnum.Proposition)
                {
                  switch (tmpSymbol.TypeId)
                  {
                    case Const.SymbolTypeEnum.FreeVariable:
                    case Const.SymbolTypeEnum.Function:
                    case Const.SymbolTypeEnum.TermQuantifier:
                      return BadRequest($"ArityType mismatch (StringsSerialNo={str.SerialNo})");
                    case Const.SymbolTypeEnum.Logic:
                    case Const.SymbolTypeEnum.Predicate:
                    case Const.SymbolTypeEnum.PropositionQuantifier:
                      continue;
                    default:
                      throw new NotImplementedException();
                  }
                }
                continue;
              }

              if (symbol.ArityFormulaTypeId != tmpSymbol.Type.FormulaTypeId)
                return BadRequest($"ArityType mismatch (StringsSerialNo={str.SerialNo})");
            }

            tmpSymbols.RemoveRange(0, symbol.Arity ?? 0);
            tmpSymbols.Insert(0, new Symbol
            {
              Character = "DUMMY",
              TypeId = symbol.TypeId
            });
          }
        }

        // FormulaChainチェック
        foreach (var chain in formula.Chains)
        {
          var fromStr = formula.Strings.Find(s => s.SerialNo == chain.FromFormulaStringSerialNo);
          if (fromStr == null)
            return BadRequest($"FormulaChain.FromFormulaStringSerialNo is not related with FormulaString");
          var fromSym = symbols.Find(s => s.Id == fromStr.SymbolId);
          if (fromSym == null)
            return BadRequest($"Symbol#{fromStr.SymbolId} is not found");
          var toStr = formula.Strings.Find(s => s.SerialNo == chain.ToFormulaStringSerialNo);
          if (toStr == null)
            return BadRequest($"FormulaChain.ToFormulaStringSerialNo is not related with FormulaString");
          var toSym = symbols.Find(s => s.Id == toStr.SymbolId);
          if (toSym == null)
            return BadRequest($"Symbol#{toStr.SymbolId} is not found");

          if (!fromSym.IsQuantifier)
            return BadRequest($"Chain FromSymbol must be quantifier");
          if (toSym.TypeId != Const.SymbolTypeEnum.BoundVariable)
            return BadRequest($"Chain ToSymbol must be BoundVariable");
        }

        foreach (var str in formula.Strings)
        {
          var symbol = symbols.Find(s => s.Id == str.SymbolId);
          if (symbol == null)
            return BadRequest($"Symbol#{str.SymbolId} is not found");

          var chain = formula.Chains.Find(c => c.ToFormulaStringSerialNo == str.SerialNo);
          if (symbol.TypeId == Const.SymbolTypeEnum.BoundVariable && chain == null)
            return BadRequest("There is a bound variable not relating FormulaChain");
        }

        _context.Formulas.Add(formula);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetFormula", new { id = formula.Id }, formula);
      }
      catch (ArgumentException exception)
      {
        return BadRequest(exception.Message);
      }
    }

    // DELETE: api/Formula/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFormula(long id)
    {
      var formula = await _context
        .Formulas
        .IgnoreAutoIncludes()
        .Include(f => f.Strings)
        .Include(f => f.Chains)
        .SingleAsync(f => f.Id == id);
      if (formula == null)
      {
        return NotFound();
      }

      _context.Formulas.Remove(formula);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool FormulaExists(long id)
    {
      return (_context.Formulas?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}
