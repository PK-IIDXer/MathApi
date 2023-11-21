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
    public async Task<ActionResult<IEnumerable<Theorem>>> GetTheorems([FromQuery] string? name, [FromQuery] string? meaning)
    {
      return await _context
        .Theorems
        .IgnoreAutoIncludes()
        .Include(t => t.Assumptions)
        .Include(t => t.Conclusions)
        .Include(t => t.Inference)
        .Include(t => t.Proofs)
        .Where(t => name == null || t.Name.Contains(name))
        .Where(t => meaning == null || t.Meaning.Contains(meaning))
        .ToListAsync();
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

    // POST: api/Theorem
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Theorem>> PostTheorem(TheoremDto theoremDto)
    {
      var theorem = theoremDto.CreateModel();
      if (theorem.Inference != null)
      {
        var labelIds = theorem.Inference.Arguments.Select(a => a.FormulaLabelId);
        var labels = await _context.FormulaLabels.Where(fl => labelIds.Contains(fl.Id)).ToListAsync();
        if (!labels.Any(ia => ia.TypeId == Const.FormulaLabelTypeEnum.Proposition))
          throw new ArgumentException("Should not use inference if there is no propositional argument");

        await SetMappingSerialNo(theorem.Inference);
      }
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

    private async Task SetMappingSerialNo(Inference inference)
    {
      int serialNo = 0;
      var mappings = new List<InferenceFormulaStructArgumentMapping>();
      var tmpLabelIds = new List<int>();

      // 仮定
      foreach (var assumption in inference.Assumptions)
      {
        var asmFormulaStructArgs = await _context.FormulaStructArguments
                                              .Where(
                                                fsa => fsa.FormulaStructId == assumption.FormulaStructId
                                              ).ToListAsync();
        foreach (var fsa in asmFormulaStructArgs)
        {
          var infArgs = inference.Arguments.Where(a => a.FormulaLabelId == fsa.LabelId);
          if (infArgs.Count() != 1)
            throw new ArgumentException("invalid FormulaLabelId in inference.Arguments");
          var infArg = infArgs.First();
          mappings.Add(new InferenceFormulaStructArgumentMapping
          {
            SerialNo = serialNo,
            FormulaStructId = fsa.FormulaStructId,
            FormulaStructArgumentSerialNo = fsa.SerialNo,
            InferenceArgumentSerialNo = infArg.SerialNo
          });
        }
        assumption.FormulaStructArgumentMappingSerialNo = serialNo++;

        // 解消可能仮定
        if (assumption.DissolutableAssumption == null)
          continue;
        var disFormulaStructArgs = await _context.FormulaStructArguments
                                                 .Where(
                                                   fsa => fsa.FormulaStructId == assumption.DissolutableAssumption.FormulaStructId
                                                 ).ToListAsync();
        foreach (var fsa in disFormulaStructArgs)
        {
          var infArgs = inference.Arguments.Where(a => a.FormulaLabelId == fsa.LabelId);
          if (infArgs.Count() != 1)
            throw new ArgumentException("invalid FormulaLabelId in inference.Arguments");
          var infArg = infArgs.First();
          mappings.Add(new InferenceFormulaStructArgumentMapping
          {
            SerialNo = serialNo,
            FormulaStructId = fsa.FormulaStructId,
            FormulaStructArgumentSerialNo = fsa.SerialNo,
            InferenceArgumentSerialNo = infArg.SerialNo
          });
        }
        assumption.DissolutableAssumption.FormulaStructArgumentMappingSerialNo = serialNo++;
      }

      // 結論
      var formulaStructArgs = await _context
        .FormulaStructArguments
        .IgnoreAutoIncludes()
        .Where(
          fsa => fsa.FormulaStructId == inference.Conclusion.FormulaStructId
        ).ToListAsync();
      foreach (var fsa in formulaStructArgs)
      {
        var infArgs = inference.Arguments.Where(a => a.FormulaLabelId == fsa.LabelId);
        if (infArgs.Count() != 1)
          throw new ArgumentException("invalid FormulaLabelId in inference.Arguments");
        var infArg = infArgs.First();
        mappings.Add(new InferenceFormulaStructArgumentMapping
        {
          SerialNo = serialNo,
          FormulaStructId = fsa.FormulaStructId,
          FormulaStructArgumentSerialNo = fsa.SerialNo,
          InferenceArgumentSerialNo = infArg.SerialNo
        });
      }
      inference.Conclusion.FormulaStructArgumentMappingSerialNo = serialNo++;

      inference.FormulaStructArgumentMappings = mappings;
    }
  }
}
