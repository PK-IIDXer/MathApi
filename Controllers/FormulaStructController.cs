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
  public class FormulaStructController : ControllerBase
  {
    private readonly MathDbContext _context;

    public FormulaStructController(MathDbContext context)
    {
      _context = context;
    }

    // GET: api/FormulaStruct
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FormulaStruct>>> GetFormulaStructs()
    {
      if (_context.FormulaStructs == null)
      {
        return NotFound();
      }
      return await _context.FormulaStructs.ToListAsync();
    }

    // GET: api/FormulaStruct/5
    [HttpGet("{id}")]
    public async Task<ActionResult<FormulaStruct>> GetFormulaStruct(long id)
    {
      if (_context.FormulaStructs == null)
      {
        return NotFound();
      }
      var formulaStruct = await _context.FormulaStructs.FindAsync(id);

      if (formulaStruct == null)
      {
        return NotFound();
      }

      return formulaStruct;
    }

    // PUT: api/FormulaStruct/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutFormulaStruct(long id, FormulaStruct formulaStruct)
    {
      if (id != formulaStruct.Id)
      {
        return BadRequest();
      }

      _context.Entry(formulaStruct).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!FormulaStructExists(id))
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

    // POST: api/FormulaStruct
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<FormulaStruct>> PostFormulaStruct(FormulaStructDto dto)
    {
      if (_context.FormulaStructs == null)
      {
        return Problem("Entity set 'MathDbContext.FormulaStructs'  is null.");
      }
      var formulaStruct = dto.CreateModel();
      var symbols = await _context.Symbols.Where(s => formulaStruct.Strings.Select(st => st.SymbolId).Contains(s.Id)).ToListAsync();
      var labels = await _context.FormulaLabels.Where(l => formulaStruct.Arguments.Select(a => a.LabelId).Contains(l.Id)).ToListAsync();

      var tmpStructStrings = new List<FormulaStructString>();
      for (var i = formulaStruct.Strings.Count - 1; i >= 0; i--)
      {
        var str = formulaStruct.Strings[i];
        if (!str.SymbolId.HasValue)
        {
          var arg = formulaStruct.Arguments.Find(a => a.SerialNo == str.ArgumentSerialNo);
          if (arg == null)
            return BadRequest($"ArgumentSerialNo mismatch (StringsSerialNo={str.SerialNo})");

          var label = labels.Find(l => l.Id == arg.LabelId);
          if (label == null)
            return BadRequest($"LabelId#{arg.LabelId} is not found (StringsSerialNo={str.SerialNo})");

          if (str.Substitutions.Count > 0)
          {
            if (label.TypeId != Const.FormulaLabelTypeEnum.Proposition)
              return BadRequest($"Only Proposition Label can have Substitutions (StringsSerialNo={str.SerialNo})");
          }

          foreach (var ss in str.Substitutions)
          {
            var argFr = formulaStruct.Arguments.Find(a => a.SerialNo == ss.ArgumentFromSerialNo);
            if (argFr == null)
              return BadRequest($"Substitution ArgumentFromSerialNo mismatch (StringsSerialNo={str.SerialNo})");

            var labelFr = labels.Find(l => l.Id == argFr.LabelId);
            if (labelFr == null)
              return BadRequest($"(Substitution) LabelId#{arg.LabelId} is not found (StringsSerialNo={str.SerialNo})");

            if (labelFr.TypeId != Const.FormulaLabelTypeEnum.FreeVariable)
              return BadRequest("Substitution From must be FreeVariable (StringsSerialNo={str.SerialNo})");

            var argTo = formulaStruct.Arguments.Find(a => a.SerialNo == ss.ArgumentToSerialNo);
            if (argTo == null)
              return BadRequest($"Substitution ArgumentToSerialNo mismatch (StringsSerialNo={str.SerialNo})");

            var labelTo = labels.Find(l => l.Id == argTo.LabelId);
            if (labelTo == null)
              return BadRequest($"(Substitution) LabelId#{arg.LabelId} is not found (StringsSerialNo={str.SerialNo})");

            if (labelTo.TypeId != Const.FormulaLabelTypeEnum.FreeVariable && labelTo.TypeId != Const.FormulaLabelTypeEnum.Term)
              return BadRequest("Substitution From must be FreeVariable or Term (StringsSerialNo={str.SerialNo})");
          }

          tmpStructStrings.Insert(0, formulaStruct.Strings[i]);
        }
        else
        {
          var symbol = symbols.Find(s => s.Id == str.SymbolId);
          if (symbol == null)
            return BadRequest($"SymbolId#{str.SymbolId} (StringsSerialNo={str.SerialNo}) is not found");

          if (symbol.Arity > tmpStructStrings.Count)
            return BadRequest($"argument count of symbol#{symbol.Id} (StringsSerialNo={str.SerialNo}) is invalid");

          if (symbol.IsQuantifier)
          {
            if (!str.BoundArgumentSerialNo.HasValue)
              return BadRequest($"Quantifier must have a BoundArgument (StringsSerialNo={str.SerialNo})");

            var boundVarArg = formulaStruct.Arguments.Find(a => a.SerialNo == str.BoundArgumentSerialNo);
            if (boundVarArg == null)
              return BadRequest($"ArgumentSerialNo is mismatch (StringsSerialNo={str.SerialNo})");

            var boundLabel = labels.Find(l => l.Id == boundVarArg.LabelId);
            if (boundLabel == null)
              return BadRequest($"FormulaLabel#{boundVarArg.LabelId} is not found (StringsSerialNo={str.SerialNo})");

            if (boundLabel.TypeId != Const.FormulaLabelTypeEnum.FreeVariable)
              return BadRequest($"BoundVariable must be FreeVariable (StringsSerialNo={str.SerialNo})");
          }

          for (var j = 0; j < symbol.Arity; j++)
          {
            var tmpFss = tmpStructStrings[j];
            var dummy = tmpFss.Symbol;
            if (dummy != null)
            {
              if (symbol.ArityFormulaTypeId == Const.FormulaTypeEnum.Term)
              {
                return
                  dummy.TypeId
                switch
                {
                  Const.SymbolTypeEnum.Logic
                    or Const.SymbolTypeEnum.Predicate
                    or Const.SymbolTypeEnum.PropositionQuantifier
                      => BadRequest($"ArityType mismatch (StringsSerialNo={str.SerialNo})"),
                  _ => throw new NotImplementedException(),
                };
              }

              if (symbol.ArityFormulaTypeId == Const.FormulaTypeEnum.Proposition)
              {
                return dummy.TypeId switch
                {
                  Const.SymbolTypeEnum.FreeVariable
                    or Const.SymbolTypeEnum.Function
                    or Const.SymbolTypeEnum.TermQuantifier
                      => BadRequest($"ArityType mismatch (StringsSerialNo={str.SerialNo})"),
                  _ => throw new NotImplementedException(),
                };
              }
              continue;
            }

            var arg = formulaStruct.Arguments.Find(a => a.SerialNo == tmpFss.ArgumentSerialNo);
            if (arg == null)
              return BadRequest($"ArgumentSerialNo mismatch (StringsSerialNo={str.SerialNo})");

            var label = labels.Find(l => l.Id == arg.LabelId);
            if (label == null)
              return BadRequest($"LabelId#{arg.LabelId} is not found (StringsSerialNo={str.SerialNo})");

            if (symbol.ArityFormulaTypeId == Const.FormulaTypeEnum.Term)
            {
              if (label.TypeId != Const.FormulaLabelTypeEnum.Term)
                return BadRequest($"ArityType mismatch (StringsSerialNo={str.SerialNo})");
            }

            if (symbol.ArityFormulaTypeId == Const.FormulaTypeEnum.Proposition)
            {
              if (label.TypeId != Const.FormulaLabelTypeEnum.Proposition)
                return BadRequest($"ArityType mismatch (StringsSerialNo={str.SerialNo})");
            }
          }

          tmpStructStrings.RemoveRange(0, symbol.Arity ?? 0);
          tmpStructStrings.Insert(0, new FormulaStructString
          {
            Symbol = new Symbol
            {
              Character = "DUMMY",
              TypeId = symbol.TypeId
            }
          });
        }
      }

      _context.FormulaStructs.Add(formulaStruct);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetFormulaStruct", new { id = formulaStruct.Id }, formulaStruct);
    }

    // DELETE: api/FormulaStruct/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFormulaStruct(long id)
    {
      if (_context.FormulaStructs == null)
      {
        return NotFound();
      }
      var formulaStruct = await _context.FormulaStructs.FindAsync(id);
      if (formulaStruct == null)
      {
        return NotFound();
      }

      _context.FormulaStructs.Remove(formulaStruct);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool FormulaStructExists(long id)
    {
      return (_context.FormulaStructs?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}
