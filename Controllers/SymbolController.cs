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
    public async Task<ActionResult<IEnumerable<Symbol>>> GetSymbols([FromQuery] string? character, [FromQuery] Const.SymbolTypeEnum? typeId, [FromQuery] int? arity, [FromQuery] string? meaning)
    {
      return await _context
        .Symbols
        .Where(s => character == null || character.Contains(s.Character))
        .Where(s => !typeId.HasValue || s.TypeId == typeId)
        .Where(s => !arity.HasValue || s.Arity == arity)
        .Where(s => meaning == null || meaning.Contains(s.Meaning ?? ""))
        .ToListAsync();
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
      if (symbol.TypeId == Const.SymbolTypeEnum.BoundVariable)
      {
        return BadRequest("Cannot modify the bound variable");
      }

      // 論理式に使用されているかチェックし、使用されていればCharacterとMeaningのみ変更可能とする
      if (_context.FormulaStrings?.Count(fs => fs.SymbolId == id) > 0)
      {
        var oldSymbol = _context.Symbols?.FirstOrDefault(s => s.Id == id);
        if (oldSymbol != null && (
          oldSymbol.TypeId != symbol.TypeId
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

      // 束縛変数は一種類のみ登録可能
      var boundVarCnt = _context.Symbols.Count(x => x.TypeId == Const.SymbolTypeEnum.BoundVariable);
      if (boundVarCnt > 0 && symbol.TypeId == Const.SymbolTypeEnum.BoundVariable)
      {
        return BadRequest("Cannot register bound variables more than 2");
      }

      _context.Symbols.Add(symbol);
      if (SaveToFormula(symbol))
      {
        _context.Formulas.Add(new Formula
        {
          Meaning = symbol.Meaning,
          Strings = new List<FormulaString>
          {
            new() {
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

      _context.Symbols.Remove(symbol);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool SymbolExists(long id)
    {
      return (_context.Symbols?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    private static bool SaveToFormula(Symbol symbol)
    {
      if (!symbol.Arity.HasValue || symbol.Arity == 0)
      {
        return true;
      }

      return false;
    }
  }
}
