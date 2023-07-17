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
    public async Task<ActionResult<IEnumerable<Proof>>> GetProofHeads()
    {
      return await _context.Proofs.ToListAsync();
    }

    // GET: api/Proof/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Proof>> GetProofHead(long id)
    {
      var proofHead = await _context.Proofs.FindAsync(id);

      if (proofHead == null)
      {
        return NotFound();
      }

      return proofHead;
    }

    // PUT: api/Proof/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProofHead(long id, Proof proofHead)
    {
      if (id != proofHead.TheoremId)
      {
        return BadRequest();
      }

      _context.Entry(proofHead).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!ProofHeadExists(id))
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
    public async Task<ActionResult<Proof>> PostProofHead(Proof proofHead)
    {
      _context.Proofs.Add(proofHead);
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException)
      {
        if (ProofHeadExists(proofHead.TheoremId))
        {
          return Conflict();
        }
        else
        {
          throw;
        }
      }

      return CreatedAtAction("GetProofHead", new { id = proofHead.TheoremId }, proofHead);
    }

    // DELETE: api/Proof/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProofHead(long id)
    {
      var proofHead = await _context.Proofs.FindAsync(id);
      if (proofHead == null)
      {
        return NotFound();
      }

      _context.Proofs.Remove(proofHead);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool ProofHeadExists(long id)
    {
      return (_context.Proofs?.Any(e => e.TheoremId == id)).GetValueOrDefault();
    }
  }
}
