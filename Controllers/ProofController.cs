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
  public class ProofController : ControllerBase
  {
    private readonly MathDbContext _context;

    public ProofController(MathDbContext context)
    {
      _context = context;
    }

    // GET: api/Proof
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Proof>>> GetProofs()
    {
      return await _context.Proofs.ToListAsync();
    }

    // GET: api/Proof/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Proof>> GetProof(long id)
    {
      var proof = await _context.Proofs.FindAsync(id);

      if (proof == null)
      {
        return NotFound();
      }

      return proof;
    }

    // PUT: api/Proof/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProof(long id, Proof proof)
    {
      if (id != proof.TheoremId)
      {
        return BadRequest();
      }

      _context.Entry(proof).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!ProofExists(id))
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

    // POST: api/Proof
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Proof>> PostProof(Proof proof)
    {
      _context.Proofs.Add(proof);
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException)
      {
        if (ProofExists(proof.TheoremId))
        {
          return Conflict();
        }
        else
        {
          throw;
        }
      }

      return CreatedAtAction("GetProof", new { id = proof.TheoremId }, proof);
    }

    // DELETE: api/Proof/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProof(long id)
    {
      var proof = await _context.Proofs.FindAsync(id);
      if (proof == null)
      {
        return NotFound();
      }

      _context.Proofs.Remove(proof);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool ProofExists(long id)
    {
      return (_context.Proofs?.Any(e => e.TheoremId == id)).GetValueOrDefault();
    }
  }
}
