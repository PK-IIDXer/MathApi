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
  public class AxiomController : ControllerBase
  {
    private readonly MathDbContext _context;

    public AxiomController(MathDbContext context)
    {
      _context = context;
    }

    // GET: api/Axiom
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Axiom>>> GetAxioms()
    {
      return await _context.Axioms.ToListAsync();
    }

    // GET: api/Axiom/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Axiom>> GetAxiom(long id)
    {
      var axiom = await _context.Axioms.FindAsync(id);

      if (axiom == null)
      {
        return NotFound();
      }

      return axiom;
    }

    // PUT: api/Axiom/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAxiom(long id, Axiom axiom)
    {
      if (id != axiom.Id)
      {
        return BadRequest();
      }

      _context.Entry(axiom).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!AxiomExists(id))
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

    // POST: api/Axiom
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Axiom>> PostAxiom(Axiom axiom)
    {
      _context.Axioms.Add(axiom);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetAxiom", new { id = axiom.Id }, axiom);
    }

    // DELETE: api/Axiom/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAxiom(long id)
    {
      var axiom = await _context.Axioms.FindAsync(id);
      if (axiom == null)
      {
        return NotFound();
      }

      _context.Axioms.Remove(axiom);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool AxiomExists(long id)
    {
      return (_context.Axioms?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}
