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
      if (_context.Symbols == null)
      {
        return NotFound();
      }
      return await _context.Symbols.ToListAsync();
    }

    // GET: api/Symbol/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Symbol>> GetSymbol(long id)
    {
      if (_context.Symbols == null)
      {
        return NotFound();
      }
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
    public async Task<ActionResult<Symbol>> PostSymbol(Symbol symbol)
    {
      if (_context.Symbols == null)
      {
        return Problem("Entity set 'MathDbContext.Symbols'  is null.");
      }
      _context.Symbols.Add(symbol);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetSymbol", new { id = symbol.Id }, symbol);
    }

    // DELETE: api/Symbol/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSymbol(long id)
    {
      if (_context.Symbols == null)
      {
        return NotFound();
      }
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
  }
}
