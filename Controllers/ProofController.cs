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
    public async Task<ActionResult<Proof>> GetProof(long id)
    {
      var proof = await _context.Proofs.FindAsync(id);

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
          var pas = proofAssumptions.Where(pa => pa.AddedProofInference.ConclusionFormulaStruct.Equals(infAsmp.DissolutableStruct));
          if (infAsmp.IsDissoluteForce && !pas.Any())
            return BadRequest("There is no ProofAssumption in spite of IsDissoluteForce");
          foreach (var pa in pas)
          {
            var pi = pa.AddedProofInference;
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
          TreeFrom = newTreeFrom,
          TreeTo = newTreeTo
        };
        _context.ProofInferences.Add(proofInference);

        if (inference.Conclusions[0].AddAssumption)
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

      // var proof = await _context.Proofs.FindAsync(dto.TheoremId, dto.ProofSerialNo);
      // if (proof == null)
      //   return BadRequest($"Proof (TheoremId, SerialNo) = (#{dto.TheoremId}, #{dto.ProofSerialNo}) is not found");

      // var inference = await _context.Inferences.FindAsync(dto.InferenceId);
      // if (inference == null)
      //   return BadRequest($"Inference #{dto.InferenceId} is not found");

      // var prevProofInferences = await _context.ProofInferences.Where(
      //   pi => pi.TheoremId == dto.TheoremId
      //     && dto.AssumingInferenceResults
      //           .Select(air => air.ProofInferenceSerialNo)
      //           .Contains(pi.SerialNo)
      //     && !pi.NextProofInferenceSerialNo.HasValue // 未使用のProofInference
      // ).ToListAsync();
      // if (prevProofInferences.Count != dto.AssumingInferenceResults.Count)
      //   return BadRequest("AssumingInferenceResults may have invalid ProofInferenceSerialNo");

      // var proofAssumptions = await _context.ProofAssumptions.Where(
      //   pa => pa.TheoremId == dto.TheoremId
      //     && pa.ProofSerialNo == dto.ProofSerialNo
      //     && !pa.DissolutedProofInferenceSerialNo.HasValue // 未解消の仮定
      // ).ToListAsync();

      // var argFormulas = await _context.Formulas
      //   .Include(f => f.FormulaStrings)
      //   .ThenInclude(fs => fs.Symbol)
      //   .ThenInclude(s => s.Type)
      //   .Include(f => f.FormulaChains)
      //   .Where(
      //     f => dto.InferenceArgumentFormulas.Select(d => d.FormulaId).Contains(f.Id)
      //   ).ToListAsync();
      // var args = dto.InferenceArgumentFormulas.Select(d => {
      //   return new ProofInferenceArgument
      //   {
      //     TheoremId = proof.TheoremId,
      //     ProofSerialNo = proof.SerialNo,
      //     ProofInferenceSerialNo = nextProofInferenceSerialNo,
      //     SerialNo = d.SerialNo,
      //     Formula = argFormulas.FirstOrDefault(f => f.Id == d.FormulaId),
      //     FormulaId = d.FormulaId,
      //   };
      // }).ToList();

      // var inferenceResult = inference.Apply(
      //   nextProofInferenceSerialNo,
      //   prevProofInferences,
      //   proofAssumptions,
      //   args
      // );

      // var pi = new ProofInference
      // {
      //   TheoremId = proof.TheoremId,
      //   ProofSerialNo = proof.SerialNo,
      //   SerialNo = nextProofInferenceSerialNo,
      //   InferenceId = inference.Id,
      //   ConclusionFormula = inferenceResult.ConclusionFormula,
      //   ProofInferenceArguments = args
      // };
      // _context.ProofInferences.Add(pi);

      // foreach (var updatedPi in inferenceResult.UpdatedProofInferences)
      // {
      //   _context.ProofInferences.Add(updatedPi);
      //   _context.Entry(updatedPi).State = EntityState.Modified;
      // }

      // foreach (var updatedPa in inferenceResult.UpdatedProofAssumptions)
      // {
      //   _context.ProofAssumptions.Add(updatedPa);
      //   _context.Entry(updatedPa).State = EntityState.Modified;
      // }

      // // TODO: 命題変数を追加する場合の考慮
      // if (inferenceResult.AddedProofAssumption != null)
      // {
      //   var nextProofAssumptionSerialNo = await _context
      //     .ProofAssumptions
      //     .Where(
      //       pa =>
      //         pa.TheoremId == dto.TheoremId
      //         && pa.ProofSerialNo == dto.ProofSerialNo)
      //     .MaxAsync(pa => pa.SerialNo) + 1;
      //   var pa = new ProofAssumption
      //   {
      //     TheoremId = proof.TheoremId,
      //     ProofSerialNo = proof.SerialNo,
      //     SerialNo = nextProofAssumptionSerialNo,
      //     FormulaId = inferenceResult.AddedProofAssumption.FormulaId,
      //     AddedProofInferenceSerialNo = nextProofInferenceSerialNo,
      //     LastUsedProofInferenceSerialNo = nextProofInferenceSerialNo
      //   };
      //   _context.ProofAssumptions.Add(pa);
      //   _context.Entry(pa).State = EntityState.Added;
      // }
      await _context.SaveChangesAsync();
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

    private async Task<List<ProofInferenceArgument>> CreateProofInferenceArguments(ProofDto dto)
    {
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
  }
}
