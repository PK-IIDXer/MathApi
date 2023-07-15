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
    public async Task<ActionResult<IEnumerable<ProofHead>>> GetProofHeads()
    {
      return await _context.ProofHeads.ToListAsync();
    }

    // GET: api/Proof/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProofHead>> GetProofHead(long id)
    {
      var proofHead = await _context.ProofHeads.FindAsync(id);

      if (proofHead == null)
      {
        return NotFound();
      }

      return proofHead;
    }

    // PUT: api/Proof/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProofHead(long id, ProofHead proofHead)
    {
      if (id != proofHead.Id)
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
    public async Task<ActionResult<ProofHead>> PostProofHead(ProofHead proofHead)
    {
      _context.ProofHeads.Add(proofHead);
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException)
      {
        if (ProofHeadExists(proofHead.Id))
        {
          return Conflict();
        }
        else
        {
          throw;
        }
      }

      return CreatedAtAction("GetProofHead", new { id = proofHead.Id }, proofHead);
    }

    // DELETE: api/Proof/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProofHead(long id)
    {
      var proofHead = await _context.ProofHeads.FindAsync(id);
      if (proofHead == null)
      {
        return NotFound();
      }

      _context.ProofHeads.Remove(proofHead);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool ProofHeadExists(long id)
    {
      return (_context.ProofHeads?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}
