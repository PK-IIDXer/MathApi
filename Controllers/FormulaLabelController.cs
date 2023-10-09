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
  public class FormulaLabelController : ControllerBase
  {
    private readonly MathDbContext _context;

    public FormulaLabelController(MathDbContext context)
    {
      _context = context;
    }

    // GET: api/FormulaLabel
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FormulaLabel>>> GetFormulaLabels(
      [FromQuery] string? text,
      [FromQuery] Const.FormulaLabelTypeEnum? typeId
    )
    {
      if (_context.FormulaLabels == null)
      {
        return NotFound();
      }
      return await _context
        .FormulaLabels
        .Where(fl => text == null || text.Contains(fl.Text))
        .Where(fl => !typeId.HasValue || fl.TypeId == typeId)
        .ToListAsync();
    }

    // GET: api/FormulaLabel/5
    [HttpGet("{id}")]
    public async Task<ActionResult<FormulaLabel>> GetFormulaLabel(int id)
    {
      if (_context.FormulaLabels == null)
      {
        return NotFound();
      }
      var formulaLabel = await _context.FormulaLabels.FindAsync(id);

      if (formulaLabel == null)
      {
        return NotFound();
      }

      return formulaLabel;
    }

    // PUT: api/FormulaLabel/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutFormulaLabel(int id, FormulaLabel formulaLabel)
    {
      if (id != formulaLabel.Id)
      {
        return BadRequest();
      }

      _context.Entry(formulaLabel).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!FormulaLabelExists(id))
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

    // POST: api/FormulaLabel
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<FormulaLabel>> PostFormulaLabel(FormulaLabelDto dto)
    {
      if (_context.FormulaLabels == null)
      {
        return Problem("Entity set 'MathDbContext.FormulaLabels'  is null.");
      }
      var formulaLabel = dto.CreateModel();
      _context.FormulaLabels.Add(formulaLabel);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetFormulaLabel", new { id = formulaLabel.Id }, formulaLabel);
    }

    // DELETE: api/FormulaLabel/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFormulaLabel(int id)
    {
      if (_context.FormulaLabels == null)
      {
        return NotFound();
      }
      var formulaLabel = await _context.FormulaLabels.FindAsync(id);
      if (formulaLabel == null)
      {
        return NotFound();
      }

      _context.FormulaLabels.Remove(formulaLabel);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool FormulaLabelExists(int id)
    {
      return (_context.FormulaLabels?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}
