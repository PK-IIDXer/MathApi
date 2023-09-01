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

    [HttpPost("define/predicate")]
    public async Task<ActionResult<Inference>> DefinePredicate(DefineSymbolDto dto)
    {
      var symbol = await _context.Symbols.FindAsync(dto.SymbolId);
      if (symbol == null)
        return NotFound($"Symbol #{dto.SymbolId} is not found");

      if (symbol.TypeId != (long)Const.SymbolType.Predicate)
        return BadRequest($"Selected symbol #{dto.SymbolId} is not predicate");

      if ((symbol.Arity ?? 0) != (dto.ArgumentSymbolIds?.Count ?? 0))
        return BadRequest($"Arity of Symbol #{dto.SymbolId} and ArgumentSymbolIds count are mismatch");

      var formula = await _context.Formulas
                                  .Include(f => f.FormulaStrings)
                                  .ThenInclude(fs => fs.Symbol)
                                  .ThenInclude(s => s.Type)
                                  .FirstAsync(f => f.Id == dto.FormulaId);
      if (formula == null)
        return NotFound($"Formula #{dto.FormulaId} is not found");

      if (formula.FormulaTypeId != (long)Const.FormulaType.Proposition)
        return BadRequest($"Selected formula #{dto.FormulaId} is not proposition");

      var args = new List<InferenceArgument>();
      var serialNo = 0;
      foreach (var freeVar in formula.FreeAndPropVariables)
      {
        if (freeVar.TypeId != (long)Const.SymbolType.FreeVariable)
          return BadRequest($"Selected formula #{dto.FormulaId} is invalid: proposition variables are contained.");

        if (!dto.ArgumentSymbolIds?.Any(item => item == freeVar.Id) ?? true)
          return BadRequest($"Free variables of formula #{dto.FormulaId} and arguments you select are mismatch");

        args.Add(new InferenceArgument
        {
          SerialNo = serialNo++,
          InferenceArgumentTypeId = (int)Const.InferenceArgumentType.Term,
          VariableSymbolId = freeVar.Id
        });
      }

      if (args.Count != symbol.Arity)
        return BadRequest($"Arity of Symbol #{dto.SymbolId} and free variables count of selected formula are mismatch");

      var assumptionFormula_intro = new List<InferenceAssumptionFormula>
      {
        new InferenceAssumptionFormula
        {
          InferenceAssumptionSerialNo = 0,
          SerialNo = 0,
          SymbolId = dto.SymbolId
        }
      };
      for (var i = 0; i < dto.ArgumentSymbolIds?.Count; i++)
      {
        assumptionFormula_intro.Add(new InferenceAssumptionFormula
        {
          InferenceAssumptionSerialNo = 0,
          SerialNo = i + 1,
          InferenceArgumentSerialNo = args.First(a => a.VariableSymbolId == dto.ArgumentSymbolIds[i]).SerialNo
        });
      }

      var assumption_intro = new InferenceAssumption
      {
        SerialNo = 0,
        DissolutionTypeId = (int)Const.InferenceAssumptionDissolutionType.None,
        InferenceAssumptionFormulas = assumptionFormula_intro
      };

      var conclusion_intro = new InferenceConclusionFormula
      {
        SerialNo = 0,
        FormulaId = dto.FormulaId
      };

      var inference_intro = new Inference
      {
        Name = $"Definition of {symbol.Character} Introduction",
        IsAssumptionAdd = false,
        Arguments = args,
        Assumptions = new List<InferenceAssumption>
        {
          assumption_intro
        },
        InferenceConclusionFormulas = new List<InferenceConclusionFormula>
        {
          conclusion_intro
        }
      };

      var assumption_elim = new InferenceAssumption
      {
        SerialNo = 0,
        DissolutionTypeId = (int)Const.InferenceAssumptionDissolutionType.None,
        InferenceAssumptionFormulas = new List<InferenceAssumptionFormula>
        {
          new InferenceAssumptionFormula
          {
            InferenceAssumptionSerialNo = 0,
            SerialNo = 0,
            FormulaId = dto.FormulaId
          }
        }
      };

      var conclusion_elim = new List<InferenceConclusionFormula>
      {
        new InferenceConclusionFormula
        {
          SerialNo = 0,
          SymbolId = dto.SymbolId
        }
      };
      for (var i = 0; i < dto.ArgumentSymbolIds?.Count; i++)
      {
        conclusion_elim.Add(new InferenceConclusionFormula
        {
          SerialNo = i + 1,
          InferenceArgumentSerialNo = args.First(a => a.VariableSymbolId == dto.ArgumentSymbolIds[i]).SerialNo
        });
      }

      var inference_elim = new Inference
      {
        Name = $"Definition of {symbol.Character} Elimination",
        IsAssumptionAdd = false,
        Arguments = args,
        Assumptions = new List<InferenceAssumption>
        {
          assumption_elim
        },
        InferenceConclusionFormulas = conclusion_elim
      };

      _context.Inferences.Add(inference_intro);
      _context.Inferences.Add(inference_elim);

      await _context.SaveChangesAsync();
      return CreatedAtAction("GetInference", new { id = inference_intro.Id }, new List<Inference> { inference_intro, inference_elim });
    }

    [HttpPost("define/function")]
    public async Task<ActionResult<Inference>> DefineFunction(DefineSymbolDto dto)
    {
      var symbol = await _context.Symbols.FindAsync(dto.SymbolId);
      if (symbol == null)
        return NotFound($"Symbol #{dto.SymbolId} is not found");

      if (symbol.TypeId != (long)Const.SymbolType.Function)
        return BadRequest($"Selected Symbol #{dto.SymbolId} is not function");

      if ((symbol.Arity ?? 0) != (dto.ArgumentSymbolIds?.Count ?? 0))
      {
        return BadRequest($"Arity of Symbol #{dto.SymbolId} and ArgumentSymbolIds count are mismatch");
      }

      var formula = await _context.Formulas
                            .Include(f => f.FormulaStrings)
                            .ThenInclude(fs => fs.Symbol)
                            .ThenInclude(s => s.Type)
                            .FirstAsync(f => f.Id == dto.FormulaId);

      if (formula == null)
        return NotFound($"Formula #{dto.FormulaId} is not found");

      if (formula.FormulaTypeId != (long)Const.FormulaType.Term)
        return BadRequest($"Selected formula #{dto.FormulaId} is not term");

      var args = new List<InferenceArgument>();
      var serialNo = 0;
      foreach (var freeVar in formula.FreeAndPropVariables)
      {
        if (freeVar.TypeId != (long)Const.SymbolType.FreeVariable)
          return BadRequest($"Selected formula #{dto.FormulaId} is invalid: proposition variables are contained.");

        if (!dto.ArgumentSymbolIds?.Any(item => item == freeVar.Id) ?? true)
          return BadRequest($"Free variables of formula #{dto.FormulaId} and arguments you select are mismatch");

        args.Add(new InferenceArgument
        {
          SerialNo = serialNo++,
          InferenceArgumentTypeId = (int)Const.InferenceArgumentType.Term,
          VariableSymbolId = freeVar.Id
        });
      }

      if (args.Count != symbol.Arity)
        return BadRequest($"Arity of Symbol #{dto.SymbolId} and free variables count of selected formula are mismatch");

      var conclusions = new List<InferenceConclusionFormula>
      {
        new InferenceConclusionFormula
        {
          SerialNo = 0,
          SymbolId = 2 // ※「=」
        },
        new InferenceConclusionFormula
        {
          SerialNo= 1,
          SymbolId = dto.SymbolId
        }
      };
      for (var i = 0; i < args.Count; i++)
      {
        conclusions.Add(new InferenceConclusionFormula
        {
          SerialNo = i + 2,
          InferenceArgumentSerialNo = args[i].SerialNo
        });
      }
      conclusions.Add(new InferenceConclusionFormula
      {
        SerialNo = 2 + args.Count,
        FormulaId = dto.FormulaId
      });

      var inference = new Inference
      {
        Name = $"Definition of ${symbol.Character}",
        IsAssumptionAdd = false,
        Arguments = args,
        InferenceConclusionFormulas = conclusions
      };

      _context.Inferences.Add(inference);
      await _context.SaveChangesAsync();
      return CreatedAtAction("GetInference", new { id = inference.Id }, new List<Inference> { inference });
    }

    private bool InferenceExists(long id)
    {
      return (_context.Inferences?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}
