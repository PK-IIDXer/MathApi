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
  public class TheoremController : ControllerBase
  {
    private readonly MathDbContext _context;

    public TheoremController(MathDbContext context)
    {
      _context = context;
    }

    // GET: api/Theorem
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Theorem>>> GetTheorems()
    {
      return await _context.Theorems.ToListAsync();
    }

    // GET: api/Theorem/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Theorem>> GetTheorem(long id)
    {
      var theorem = await _context.Theorems.FindAsync(id);

      if (theorem == null)
      {
        return NotFound();
      }

      return theorem;
    }

    // PUT: api/Theorem/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTheorem(long id, Theorem theorem)
    {
      if (id != theorem.Id)
      {
        return BadRequest();
      }

      _context.Entry(theorem).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!TheoremExists(id))
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

    // POST: api/Theorem
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Theorem>> PostTheorem(Theorem theorem)
    {
      _context.Theorems.Add(theorem);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetTheorem", new { id = theorem.Id }, theorem);
    }

    // DELETE: api/Theorem/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTheorem(long id)
    {
      var theorem = await _context.Theorems.FindAsync(id);
      if (theorem == null)
      {
        return NotFound();
      }

      _context.Theorems.Remove(theorem);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool TheoremExists(long id)
    {
      return (_context.Theorems?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}
