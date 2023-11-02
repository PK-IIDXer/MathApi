using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MathApi.Models;
using System.Numerics;

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

    // POST: api/Inference
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Inference>> PostInference(InferenceDto inferenceDto)
    {
      var inference = inferenceDto.CreateModel();
      await SetMappingSerialNo(inference);
      // debug
      Console.WriteLine(ObjectDumper.Dump(inference));
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
