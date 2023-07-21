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
    public async Task<ActionResult<Theorem>> PostTheorem(TheoremDto theoremDto)
    {
      var theorem = theoremDto.CreateModel();
      _context.Theorems.Add(theorem);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetTheorem", new { id = theorem.Id }, theorem);
    }

    [HttpPost("{id}/createInference")]
    public async Task<ActionResult<Inference>> PostInference(long id)
    {
      if (await _context.Inferences.CountAsync(i => i.TheoremId == id) > 0)
      {
        return BadRequest();
      }

      var theorem = await _context.Theorems
                                  .Include(t => t.TheoremAssumptions)
                                  .ThenInclude(ta => ta.Formula)
                                  .ThenInclude(f => f.FormulaStrings)
                                  .ThenInclude(fs => fs.Symbol)
                                  .Include(t => t.TheoremConclusions)
                                  .ThenInclude(tc => tc.Formula)
                                  .ThenInclude(f => f.FormulaStrings)
                                  .ThenInclude(fs => fs.Symbol)
                                  .FirstAsync(t => t.Id == id);
      if (theorem == null) return NotFound();

      // 推論規則引数の構築
      var inferenceArguments = new List<InferenceArgument>();
      for (var i = 0; i < theorem.FreeAndPropVariables.Count; i++)
      {
        var inferenceArgumentType = (int)Const.InferenceArgumentType.Term;
        if (theorem.FreeAndPropVariables[i].SymbolTypeId == (int)Const.SymbolType.PropositionVariable)
          inferenceArgumentType = (int)Const.InferenceArgumentType.Proposition;

        inferenceArguments.Add(new InferenceArgument
        {
          SerialNo = i,
          InferenceArgumentTypeId = inferenceArgumentType,
          PropositionVariableSymbolId = theorem.FreeAndPropVariables[i].Id
        });
      }

      // 推論規則仮定の構築
      var inferenceAssumptions = new List<InferenceAssumption>();
      for (var i = 0; i < theorem.TheoremAssumptions.Count; i++)
      {
        inferenceAssumptions.Add(new InferenceAssumption
        {
          SerialNo = i,
          InferenceAssumptionDissolutionTypeId = (int)Const.InferenceAssumptionDissolutionType.None,
          InferenceAssumptionFormulas = new List<InferenceAssumptionFormula>
          {
            new InferenceAssumptionFormula
            {
              InferenceAssumptionSerialNo = i,
              SerialNo = 0,
              FormulaId = theorem.TheoremAssumptions[i].FormulaId
            }
          }
        });
      }

      // 推論規則結論の構築
      var inferenceConclusions = new List<InferenceConclusionFormula>
      {
        new InferenceConclusionFormula
        {
          SerialNo = 0,
          FormulaId = theorem.TheoremConclusions[0].FormulaId
        }
      };

      var inference = new Inference {
        Name = theorem.Name,
        IsAssumptionAdd = false,
        TheoremId = theorem.Id,
        InferenceArguments = inferenceArguments,
        InferenceAssumptions = inferenceAssumptions,
        InferenceConclusionFormulas = inferenceConclusions
      };

      _context.Inferences.Add(inference);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetInference", new { id = inference.Id }, inference);
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
