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
    public async Task<ActionResult<Proof>> PostProof(ProofDto dto)
    {
      var nextProofInferenceSerialNo = await _context
        .ProofInferences
        .Where(
          pi =>
            pi.TheoremId == dto.TheoremId
            && pi.ProofSerialNo == dto.ProofSerialNo)
        .MaxAsync(pi => pi.SerialNo) + 1;

      // TODO: Include
      var proof = await _context.Proofs.FindAsync(dto.TheoremId, dto.ProofSerialNo);
      if (proof == null)
        return BadRequest($"Proof (TheoremId, SerialNo) = (#{dto.TheoremId}, #{dto.ProofSerialNo}) is not found");

      // TODO: Include
      var inference = await _context.Inferences.FindAsync(dto.InferenceId);
      if (inference == null)
        return BadRequest($"Inference #{dto.InferenceId} is not found");

      // TODO: Include
      var prevProofInferences = await _context.ProofInferences.Where(
        pi => pi.TheoremId == dto.TheoremId
          && dto.AssumingInferenceResults
                .Select(air => air.ProofInferenceSerialNo)
                .Contains(pi.SerialNo)
      ).ToListAsync();

      var proofAssumptions = await _context.ProofAssumptions.Where(
        pa => pa.TheoremId == dto.TheoremId
          && pa.ProofSerialNo == dto.ProofSerialNo
          && !pa.DissolutedProofInferenceSerialNo.HasValue // 未解消の仮定
      ).ToListAsync();

      // var inferenceResult = inference.Apply(
      //   nextProofInferenceSerialNo,
      //   proof,
      //   prevProofInferences,
      //   proofAssumptions,
      // );

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

    // public async Task<string?> ValidateDto(ProofDto dto)
    // {
    //   var proof = await _context
    //   .Proofs
    //   .Include(
    //     p => p.ProofInferences
    //           .Where(pi => !pi.NextProofInferenceSerialNo.HasValue)
    //   )
    //   .FirstOrDefaultAsync(
    //     p => p.TheoremId == dto.TheoremId
    //          && p.SerialNo == dto.ProofSerialNo
    //   );

    //   var inference = await _context
    //   .Inferences
    //   .Include(i => i.InferenceArguments)
    //   .Include(i => i.InferenceAssumptions)
    //   .Include(i => i.InferenceConclusionFormulas)
    //   .FirstAsync(i => i.Id == dto.InferenceId);

    //   if (inference.InferenceArguments.Count != dto.InferenceArgumentFormulas.Count)
    //     return "Argument couunt mismatch.";

    //   if (inference.InferenceAssumptions.Count != dto.AssumingInferenceResults.Count)
    //     return "Assumption count mismatch.";

      

    //   return null;
    // }
  }
}
