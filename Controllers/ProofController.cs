using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MathApi.Models;
using System.Runtime.CompilerServices;

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
    public async Task<ActionResult<Proof>> GetProof(long theoremId, long serialNo)
    {
      var proof = await _context.Proofs.FindAsync(theoremId, serialNo);

      if (proof == null)
      {
        return NotFound();
      }

      return proof;
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

      var proof = await CheckAndGetProof(dto);

      var theorem = await _context.Theorems.FindAsync(dto.TheoremId);
      if (theorem == null)
        return NotFound($"Theorem#{dto.TheoremId} is not found");

      var proofAssumptions = await _context.ProofAssumptions.ToListAsync();

      var isStruct = theorem.Inference != null;

      var inferenceArguments = await CreateProofInferenceArguments(dto);

      var inference = await _context.Inferences.FirstOrDefaultAsync(i => i.Id == dto.InferenceId);
      if (inference == null)
        return NotFound($"Inference#{dto.InferenceId} is not found");

      var assumptionPis = await _context
        .ProofInferences
        .Where(pi =>
          pi.TheoremId == dto.TheoremId
            && dto.ProofInferenceSerialNos.Contains(pi.SerialNo))
        .ToListAsync();

      if (assumptionPis.Count != dto.ProofInferenceSerialNos.Count)
        return NotFound("some ProofInference is not found");

      foreach (var asmp in assumptionPis)
      {
        if (await _context.ProofInferences.AnyAsync(pi => pi.TreeFrom < asmp.TreeFrom && asmp.TreeTo < pi.TreeTo))
          throw new ArgumentException("Used ProofInference is set");
      }

      var newTreeFrom = 0L;
      var newTreeTo = 0L;
      if (assumptionPis.Count == 0)
      {
        newTreeFrom = await _context.ProofInferences.MaxAsync(pi => pi.TreeTo) + 1;
        newTreeTo = newTreeFrom + 1;
      }
      else
      {
        newTreeFrom = assumptionPis.Min(a => a.TreeFrom);
        newTreeTo = assumptionPis.Max(a => a.TreeTo) + 2;
      }

      if (isStruct)
      {
        var ret = CreateProofDataOnIsStruct(
          dto,
          inferenceArguments,
          inference,
          assumptionPis,
          proofAssumptions,
          nextProofInferenceSerialNo,
          newTreeFrom,
          newTreeTo
        );
        if (ret != null)
          return ret;
      }
      else
      {
        var ret = CreateProofDataOnIsNotStruct(
          dto,
          inferenceArguments,
          inference,
          assumptionPis,
          proofAssumptions,
          nextProofInferenceSerialNo,
          newTreeFrom,
          newTreeTo
        );
        if (ret != null)
          return ret;
      }

      await _context
        .ProofInferences
        .Where(pi => pi.TreeFrom >= newTreeFrom && pi.TreeTo < newTreeTo)
        .ExecuteUpdateAsync(s => s
          .SetProperty(pi => pi.TreeFrom, pi => pi.TreeFrom + 1)
          .SetProperty(pi => pi.TreeTo, pi => pi.TreeTo + 1));
      await _context
        .ProofInferences
        .Where(pi => pi.TreeTo >= newTreeTo)
        .ExecuteUpdateAsync(s => s
          .SetProperty(pi => pi.TreeFrom, pi => pi.TreeFrom + 2)
          .SetProperty(pi => pi.TreeTo, pi => pi.TreeTo + 2));

      await _context.SaveChangesAsync();
      return CreatedAtAction("GetProof", new { theoremId = proof.TheoremId, serialNo = proof.SerialNo }, proof);
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

    private async Task<List<ProofInferenceArgument>> CreateProofInferenceArguments(ProofDto dto)
    {
      if (dto.ProofAssumptionSerialNo.HasValue)
      {
        if (!ValidateOnUsingAssumptionReassume(dto))
          throw new ArgumentException("Invalid request params where ProofAssumptionSerialNo is null");
        var arg = await _context.ProofAssumptions.FindAsync(dto.TheoremId, dto.ProofSerialNo, dto.ProofAssumptionSerialNo)
          ?? throw new ArgumentException($"ProofAssumption is not found: (TheoremId, ProofSerialNo, SerialNo)=({dto.TheoremId},{dto.ProofSerialNo},{dto.ProofAssumptionSerialNo})");
        if (arg.AddedProofInference == null)
          throw new ArgumentException("Include ProofAssumption.AddedProofInference");
        return new List<ProofInferenceArgument>
        {
          new()
          {
            TheoremId = dto.TheoremId,
            ProofInferenceSerialNo = dto.ProofSerialNo,
            SerialNo = 0,
            FormulaId = arg.AddedProofInference.ConclusionFormulaId,
            Formula = arg.AddedProofInference.ConclusionFormula,
            FormulaStructId = arg.AddedProofInference.ConclusionFormulaStructId,
            FormulaStruct = arg.AddedProofInference.ConclusionFormulaStruct
          }
        };
      }

      var formulas = await _context.Formulas.Where(f => dto.InferenceArguments.Select(d => d.FormulaId).Contains(f.Id)).ToListAsync();
      var formulaStructs = await _context.FormulaStructs.Where(fs => dto.InferenceArguments.Select(d => d.FormulaStructId).Contains(fs.Id)).ToListAsync();

      var inferenceArguments = new List<ProofInferenceArgument>();
      // 連番チェック用変数
      var serialNo = 0;
      foreach (var agDto in dto.InferenceArguments.OrderBy(ia => ia.SerialNo))
      {
        if (agDto.SerialNo != serialNo++)
          throw new ArgumentException("Inputted ProofInferenceArgument.SerialNo is not serial");

        var formula = formulas.FirstOrDefault(f => f.Id == agDto.FormulaId);
        if (agDto.FormulaId.HasValue)
        {
          if (formula == null)
            throw new ArgumentException($"Formula#{agDto.FormulaId} is not found");
        }
        var formulaStruct = formulaStructs.FirstOrDefault(fs => fs.Id == agDto.FormulaStructId);
        if (agDto.FormulaStructId.HasValue)
        {
          if (formulaStruct == null)
            throw new ArgumentException($"FormulaStruct#{agDto.FormulaStructId} is not found");
        }

        inferenceArguments.Add(new ProofInferenceArgument
        {
          TheoremId = dto.TheoremId,
          ProofInferenceSerialNo = dto.ProofSerialNo,
          SerialNo = agDto.SerialNo,
          FormulaId = agDto.FormulaId,
          Formula = formula,
          FormulaStructId = agDto.FormulaStructId,
          FormulaStruct = formulaStruct
        });
      }

      return inferenceArguments;
    }

    private async Task<Proof> CheckAndGetProof(ProofDto dto)
    {
      if (!await _context.Proofs.AnyAsync(p => p.TheoremId == dto.TheoremId))
      {
        if (dto.ProofSerialNo != 0)
          throw new ArgumentException("ProofSerialNo should be set 0 on the first proof");
        return new Proof
        {
          TheoremId = dto.TheoremId,
          SerialNo = 0,
        };
      }

      if (!await _context.Proofs.AnyAsync(p => p.TheoremId == dto.TheoremId && p.SerialNo == dto.ProofSerialNo))
      {
        var maxProofSerialNo = await _context.Proofs.Where(p => p.TheoremId == dto.TheoremId).MaxAsync(p => p.SerialNo);
        if (maxProofSerialNo + 1 != dto.ProofSerialNo)
          throw new ArgumentException("ProofSerialNo is not next of the max of Proof's one: current max = " + maxProofSerialNo);
        return new Proof
        {
          TheoremId = dto.TheoremId,
          SerialNo = dto.ProofSerialNo
        };
      }

      return await _context.Proofs.FirstAsync(p => p.TheoremId == dto.TheoremId && p.SerialNo == dto.ProofSerialNo);
    }

    private ObjectResult? CreateProofDataOnIsStruct(
      ProofDto dto,
      List<ProofInferenceArgument> inferenceArguments,
      Inference inference,
      List<ProofInference> assumptionPis,
      List<ProofAssumption> proofAssumptions,
      long nextProofInferenceSerialNo,
      long newTreeFrom,
      long newTreeTo
    )
    {
      var formulaStructs = inferenceArguments.Select(ia => ia.FormulaStruct ?? throw new Exception("想定外")).ToList();
      var inferenceResult = inference.Apply(formulaStructs);
      if (inferenceResult.AssumptionFormulaStructs.Count != assumptionPis.Count)
        return BadRequest("Assumption count mismatch");
      foreach (var asmp in assumptionPis)
      {
        if (!inferenceResult.AssumptionFormulaStructs.Any(a => a.Equals(asmp)))
          return BadRequest($"Illegal ProofInferenceSerialNo is set: #{asmp.SerialNo}");
      }
      foreach (var infAsmp in inferenceResult.AssumptionFormulaStructs)
      {
        var asmpPi = assumptionPis.Find(a => a.ConclusionFormulaStruct?.Equals(infAsmp.FormulaStruct) ?? false);
        if (asmpPi == null)
          return BadRequest($"Some InferenceArgument is not enough");
        var pas = proofAssumptions.Where(pa =>
        {
          if (pa.AddedProofInference == null)
            return false;
          if (pa.AddedProofInference.ConclusionFormulaStruct == null)
            return false;
          if (infAsmp.DissolutableStruct == null)
            return false;
          return pa.AddedProofInference.ConclusionFormulaStruct.Equals(infAsmp.DissolutableStruct);
        });
        if (infAsmp.IsDissoluteForce && !pas.Any())
          return BadRequest("There is no ProofAssumption in spite of IsDissoluteForce");
        foreach (var pa in pas)
        {
          var pi = pa.AddedProofInference;
          if (pi == null)
            return BadRequest("Include ProofAssumption.AddedProofInference");
          if (!assumptionPis.All(aPi => aPi.TreeFrom < pi.TreeFrom && pi.TreeTo < aPi.TreeTo))
            return BadRequest("There is dissoluting ProofAssumption of assumed at not argumented assumptions");
          pa.DissolutedProofInferenceSerialNo = nextProofInferenceSerialNo;

          _context.ProofAssumptions.Add(pa);
        }
      }
      var proofInference = new ProofInference
      {
        TheoremId = dto.TheoremId,
        ProofSerialNo = dto.ProofSerialNo,
        SerialNo = nextProofInferenceSerialNo,
        InferenceId = dto.InferenceId,
        ConclusionFormulaStruct = inferenceResult.ConclusionFormulaStructs[0],
        ProofAssumptionSerialNo = dto.ProofAssumptionSerialNo,
        TreeFrom = newTreeFrom,
        TreeTo = newTreeTo
      };
      _context.ProofInferences.Add(proofInference);

      if (inference.Conclusion.AddAssumption && !dto.ProofAssumptionSerialNo.HasValue)
      {
        var proofAssumption = new ProofAssumption
        {
          TheoremId = dto.TheoremId,
          ProofSerialNo = dto.ProofSerialNo,
          SerialNo = proofAssumptions.Max(a => a.ProofSerialNo) + 1,
          AddedProofInferenceSerialNo = nextProofInferenceSerialNo
        };
        _context.ProofAssumptions.Add(proofAssumption);
      }

      var argumentSerialNo = 0;
      var arguments = dto.InferenceArguments.Select(a => new ProofInferenceArgument
      {
        TheoremId = dto.TheoremId,
        ProofSerialNo = dto.ProofSerialNo,
        ProofInferenceSerialNo = nextProofInferenceSerialNo,
        SerialNo = argumentSerialNo++,
        FormulaStructId = a.FormulaStructId
      });
      _context.ProofInferenceArguments.AddRange(arguments);
      return null;
    }

    private ObjectResult? CreateProofDataOnIsNotStruct(
      ProofDto dto,
      List<ProofInferenceArgument> inferenceArguments,
      Inference inference,
      List<ProofInference> assumptionPis,
      List<ProofAssumption> proofAssumptions,
      long nextProofInferenceSerialNo,
      long newTreeFrom,
      long newTreeTo
    )
    {
      var formulas = inferenceArguments.Select(ia => ia.Formula ?? throw new Exception("想定外")).ToList();
      var inferenceResult = inference.Apply(formulas);
      if (inferenceResult.AssumptionFormulas.Count != assumptionPis.Count)
        return BadRequest("Assumption count mismatch");
      foreach (var asmp in assumptionPis)
      {
        if (!inferenceResult.AssumptionFormulas.Any(a => a.Equals(asmp)))
          return BadRequest($"Illegal ProofInferenceSerialNo is set: #{asmp.SerialNo}");
      }
      foreach (var infAsmp in inferenceResult.AssumptionFormulas)
      {
        var asmpPi = assumptionPis.Find(a => a.ConclusionFormula?.Equals(infAsmp.Formula) ?? false);
        if (asmpPi == null)
          return BadRequest($"Some InferenceArgument is not enough");
        var pas = proofAssumptions.Where(pa =>
        {
          if (pa.AddedProofInference == null)
            return false;
          if (pa.AddedProofInference.ConclusionFormula == null)
            return false;
          if (infAsmp.Dissolutable == null)
            return false;
          return pa.AddedProofInference.ConclusionFormula.Equals(infAsmp.Dissolutable);
        });
        if (infAsmp.IsDissoluteForce && !pas.Any())
          return BadRequest("There is no ProofAssumption in spite of IsDissoluteForce");
        foreach (var pa in pas)
        {
          var pi = pa.AddedProofInference;
          if (pi == null)
            return BadRequest("Include ProofAssumption.AddedProofInference");
          if (!assumptionPis.All(aPi => aPi.TreeFrom < pi.TreeFrom && pi.TreeTo < aPi.TreeTo))
            return BadRequest("There is dissoluting ProofAssumption of assumed at not argumented assumptions");
          pa.DissolutedProofInferenceSerialNo = nextProofInferenceSerialNo;

          _context.ProofAssumptions.Add(pa);
        }
      }
      var proofInference = new ProofInference
      {
        TheoremId = dto.TheoremId,
        ProofSerialNo = dto.ProofSerialNo,
        SerialNo = nextProofInferenceSerialNo,
        InferenceId = dto.InferenceId,
        ConclusionFormula = inferenceResult.ConclusionFormulas[0],
        ProofAssumptionSerialNo = dto.ProofAssumptionSerialNo,
        TreeFrom = newTreeFrom,
        TreeTo = newTreeTo
      };
      _context.ProofInferences.Add(proofInference);

      if (inference.Conclusion.AddAssumption && !dto.ProofAssumptionSerialNo.HasValue)
      {
        var proofAssumption = new ProofAssumption
        {
          TheoremId = dto.TheoremId,
          ProofSerialNo = dto.ProofSerialNo,
          SerialNo = proofAssumptions.Max(a => a.ProofSerialNo) + 1,
          AddedProofInferenceSerialNo = nextProofInferenceSerialNo
        };
        _context.ProofAssumptions.Add(proofAssumption);
      }

      var argumentSerialNo = 0;
      var arguments = dto.InferenceArguments.Select(a => new ProofInferenceArgument
      {
        TheoremId = dto.TheoremId,
        ProofSerialNo = dto.ProofSerialNo,
        ProofInferenceSerialNo = nextProofInferenceSerialNo,
        SerialNo = argumentSerialNo++,
        FormulaStructId = a.FormulaStructId
      });
      _context.ProofInferenceArguments.AddRange(arguments);
      return null;
    }

    private static bool ValidateOnUsingAssumptionReassume(ProofDto dto)
    {
      if (!dto.ProofAssumptionSerialNo.HasValue)
        return true;

      // TODO: 基本的な推論規則のIDを定数化
      if (dto.InferenceId != 1)  // 推論規則ID=1:仮定
      {
        return false;
      }

      if (dto.InferenceArguments.Count > 0)
      {
        return false;
      }

      return true;
    }
  }
}
