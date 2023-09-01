using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MathApi.Models;
using MathApi.Commons;

namespace MathApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FormulaController : ControllerBase
  {
    private readonly MathDbContext _context;

    public FormulaController(MathDbContext context)
    {
      _context = context;
    }

    // GET: api/Formula
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Formula>>> GetFormulas()
    {
      return await _context.Formulas.ToListAsync();
    }

    // GET: api/Formula/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Formula>> GetFormula(long id)
    {
      var formula = await _context.Formulas.FindAsync(id);

      if (formula == null)
      {
        return NotFound();
      }

      return formula;
    }

    // PUT: api/Formula/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutFormula(long id, Formula formula)
    {
      // TODO: 実装

      if (id != formula.Id)
      {
        return BadRequest();
      }

      _context.Entry(formula).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!FormulaExists(id))
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

    // POST: api/Formula
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Formula>> PostFormula(FormulaDto postParam)
    {
      try
      {
        var formula = await CreateFormulaFromPostParam(postParam);
        _context.Formulas.Add(formula);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetFormula", new { id = formula.Id }, formula);
      }
      catch (ArgumentException exception)
      {
        return BadRequest(exception.Message);
      }
    }

    // DELETE: api/Formula/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFormula(long id)
    {
      // TODO: 推論規則、公理、定理に使用されている場合、削除不可
      var formula = await _context.Formulas.FindAsync(id);
      if (formula == null)
      {
        return NotFound();
      }

      _context.Formulas.Remove(formula);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool FormulaExists(long id)
    {
      return (_context.Formulas?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    /// <summary>
    /// POSTパラメータからFormulaオブジェクトを作成
    /// </summary>
    /// <param name="postParam">POSTパラメータ</param>
    /// <returns>Formulaオブジェクト</returns>
    /// <exception cref="InvalidDataException"></exception>
    private async Task<Formula> CreateFormulaFromPostParam(FormulaDto postParam)
    {
      // 一文字目を取得
      var firstSymbol = await _context.Symbols
                                      .Include(s => s.Type)
                                      .Include(s => s.ArityFormulaType)
                                      .FirstAsync(s => s.Id == postParam.FirstSymbolId)
                        ?? throw new InvalidDataException("first symbol is not found");

      // 一文字目が量化記号のとき、束縛変数を取得
      Formula? boundVariable = null;
      if (firstSymbol.IsQuantifier)
      {
        var boundVarSymbl = await _context.Symbols
                                          .Include(s => s.Type)
                                          .Include(s => s.ArityFormulaType)
                                          .FirstAsync(s => s.Id == postParam.BoundVariableId)
                            ?? throw new InvalidDataException("bound variable is not found");
        boundVariable = new Formula
        {
          FormulaStrings = new List<FormulaString> {
            new() {
              SerialNo = 0,
              Symbol = boundVarSymbl,
              SymbolId = boundVarSymbl.Id
            }
          }
        };
      }

      // 引数の論理式を取得
      var argFormulas = new List<Formula>();
      if (postParam.ArgumentedFormulaIds.Any())
      {
        argFormulas = await _context.Formulas
          .Include(f => f.FormulaStrings.OrderBy(fs => fs.SerialNo))
          .ThenInclude(fs => fs.Symbol)
          .ThenInclude(s => s.Type)
          .Include(f => f.FormulaChains)
          .Where(f => postParam.ArgumentedFormulaIds.Contains(f.Id))
          .ToListAsync();
      }

      // 戻り値作成
      var ret = Util.ProceedFormulaConstruction(
        firstSymbol,
        boundVariable,
        argFormulas
      );
      ret.Meaning = postParam.Meaning;

      _context.Entry(ret).State = EntityState.Added;
      foreach (var fs in ret.FormulaStrings)
      {
        _context.Entry(fs).State = EntityState.Added;
      }
      foreach (var fc in ret.FormulaChains)
      {
        _context.Entry(fc).State = EntityState.Added;
      }
      return ret;
    }
  }
}
