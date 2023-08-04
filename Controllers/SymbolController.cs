using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MathApi.Models;

namespace MathApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SymbolController : ControllerBase
  {
    private readonly MathDbContext _context;

    public SymbolController(MathDbContext context)
    {
      _context = context;
    }

    // GET: api/Symbol
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Symbol>>> GetSymbols()
    {
      return await _context.Symbols.ToListAsync();
    }

    // GET: api/Symbol/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Symbol>> GetSymbol(long id)
    {
      var symbol = await _context.Symbols.FindAsync(id);

      if (symbol == null)
      {
        return NotFound();
      }

      return symbol;
    }

    // PUT: api/Symbol/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSymbol(long id, Symbol symbol)
    {
      if (id != symbol.Id)
      {
        return BadRequest();
      }

      // □は編集不可
      if (symbol.SymbolTypeId == (long)Const.SymbolType.BoundVariable)
      {
        return BadRequest("Cannot modify the bound variable");
      }

      // 論理式に使用されているかチェックし、使用されていればCharacterとMeaningのみ変更可能とする
      if (_context.FormulaStrings?.Count(fs => fs.SymbolId == id) > 0)
      {
        var oldSymbol = _context.Symbols?.FirstOrDefault(s => s.Id == id);
        if (oldSymbol != null && (
          oldSymbol.SymbolTypeId != symbol.SymbolTypeId
          || oldSymbol.Arity != symbol.Arity
          || oldSymbol.ArityFormulaTypeId != symbol.ArityFormulaTypeId))
        {
          return BadRequest("Cannot modify SymbolType, Arity, and ArityFormulaType if the symbol is contained in some Formulas");
        }
      }

      _context.Entry(symbol).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!SymbolExists(id))
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

    // POST: api/Symbol
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Symbol>> PostSymbol(SymbolDto symbolDto)
    {
      var symbol = symbolDto.CreateModel();
      var valid = Validate(symbol);
      if (valid != null)
      {
        return BadRequest(valid);
      }

      // 束縛変数は一種類のみ登録可能
      var boundVarCnt = _context.Symbols.Count(x => x.SymbolTypeId == (long)Const.SymbolType.BoundVariable);
      if (boundVarCnt > 0 && symbol.SymbolTypeId == (long)Const.SymbolType.BoundVariable)
      {
        return BadRequest("Cannot register bound variables more than 2");
      }

      _context.Symbols.Add(symbol);
      if (SaveToFormula(symbol))
      {
        _context.Formulas.Add(new Formula
        {
          Meaning = symbol.Meaning,
          FormulaStrings = new List<FormulaString>
          {
            new FormulaString
            {
              SerialNo = 0,
              Symbol = symbol
            }
          }
        });
      }
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetSymbol", new { id = symbol.Id }, symbol);
    }

    // DELETE: api/Symbol/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSymbol(long id)
    {
      var symbol = await _context.Symbols.FindAsync(id);
      if (symbol == null)
      {
        return NotFound();
      }

      // 論理式に使用されているかチェックし、使用されていればエラーとする
      if (await _context.FormulaStrings.AnyAsync(fs => fs.SymbolId == id))
        return BadRequest("Cannot delete if the symbol is contained in some Formulas");

      if (await _context.InferenceAssumptionFormulas.AnyAsync(iaf => iaf.SymbolId == id))
        return BadRequest("Cannot delete if the symbol is contained in some Inferences");

      if (await _context.InferenceAssumptionDissolutableAssumptionFormulas.AnyAsync(iaf => iaf.SymbolId == id))
        return BadRequest("Cannot delete if the symbol is contained in some Inferences");

      if (await _context.InferenceConclusionFormulas.AnyAsync(iaf => iaf.SymbolId == id))
        return BadRequest("Cannot delete if the symbol is contained in some Inferences");

      _context.Symbols.Remove(symbol);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool SymbolExists(long id)
    {
      return (_context.Symbols?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    private static string? Validate(Symbol symbol)
    {
      if (symbol.SymbolTypeId == (long)Const.SymbolType.TermQuantifier
        || symbol.SymbolTypeId == (long)Const.SymbolType.PropositionQuantifier)
      {
        if (symbol.Arity == 0)
        {
          return "Cannot set Arity = 0 if it is Quantifier";
        }
      }
      return null;
    }

    private static bool SaveToFormula(Symbol symbol)
    {
      if (symbol.SymbolTypeId == (long)Const.SymbolType.FreeVariable
        || symbol.SymbolTypeId == (long)Const.SymbolType.BoundVariable
        || symbol.SymbolTypeId == (long)Const.SymbolType.PropositionVariable
        || symbol.SymbolTypeId == (long)Const.SymbolType.Constant)
      {
        return true;
      }

      if (symbol.SymbolTypeId != (long)Const.SymbolType.TermQuantifier
        && symbol.SymbolTypeId != (long)Const.SymbolType.PropositionQuantifier
        && symbol.Arity == 0)
      {
        return true;
      }

      return false;
    }
  }
}
