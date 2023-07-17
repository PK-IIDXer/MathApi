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
  public class InferenceController : ControllerBase
  {
    private readonly MathDbContext _context;

    public InferenceController(MathDbContext context)
    {
      _context = context;
    }

    // GET: api/Inference
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Inference>>> GetInferences()
    {
      return await _context.Inferences.ToListAsync();
    }

    // GET: api/Inference/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Inference>> GetInference(long id)
    {
      var inference = await _context.Inferences.FindAsync(id);

      if (inference == null)
      {
        return NotFound();
      }

      return inference;
    }

    // PUT: api/Inference/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutInference(long id, Inference inference)
    {
      if (id != inference.Id)
      {
        return BadRequest();
      }

      _context.Entry(inference).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!InferenceExists(id))
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

    // POST: api/Inference
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Inference>> PostInference(InferenceDto inferenceDto)
    {
      var inference = inferenceDto.CreateModel();
      _context.Inferences.Add(inference);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetInference", new { id = inference.Id }, inference);
    }

    // DELETE: api/Inference/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInference(long id)
    {
      if (_context.Inferences == null)
      {
        return NotFound();
      }
      var inference = await _context.Inferences.FindAsync(id);
      if (inference == null)
      {
        return NotFound();
      }

      _context.Inferences.Remove(inference);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool InferenceExists(long id)
    {
      return (_context.Inferences?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    // private string? ValidateParam(InferenceDto dto)
    // {
    //   if (dto.Conclusions.Count == 0)
    //   {
    //     return "Conclusion is null";
    //   }

    //   var usedSymbols = new List<Symbol>();
    //   for (var i = 0; i < dto.Conclusions.Count; i ++)
    //   {
    //     var conclusion = dto.Conclusions.Find(c => c.SerialNo == i);
    //     if (!usedSymbols.Any(u => u.Id == conclusion.SymbolId))
    //     {
    //       usedSymbols.Add(_context.Symbols.Find(conclusion.SymbolId));
    //     }
    //     if (conclusion.)
    //   }
    // }
  }
}
