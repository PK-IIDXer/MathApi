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
    public async Task<ActionResult<Formula>> PostFormula(PostParameter postParam)
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
    /// POSTパラメータからFormulaStringリストを作成する
    /// </summary>
    /// <param name="firstSymbol">一文字目</param>
    /// <param name="argFormulaIds">POSTで渡された論理式IDのリスト</param>
    /// <param name="argFormulas">引数論理式のオブジェクトリスト</param>
    /// <param name="isQuant">一文字目が量化記号かどうか</param>
    /// <param name="boundVariable">束縛変数文字オブジェクト</param>
    /// <param name="boundId">束縛する自由変数のFormulaID</param>
    /// <returns>FormulaStringのリスト</returns>
    /// <exception cref="InvalidDataException">
    /// 量化記号を使うのに束縛変数記号がDBに登録されてないときにThrow
    /// </exception>
    private static List<FormulaString> CreateFormulaStringFromPostParam(
      Symbol firstSymbol,
      List<long> argFormulaIds,
      List<Formula> argFormulas,
      bool isQuant,
      Symbol? boundVariable,
      long? boundId
    )
    {
      long serialNo = 0;

      // 一文字目をセット
      var strings = new List<FormulaString>
      {
        new FormulaString
        {
          SerialNo = serialNo++,
          SymbolId = firstSymbol.Id,
          Symbol = firstSymbol
        }
      };

      // 引数の論理式を一文字ずつバラシてstringsに追加する
      foreach (var argFormulaId in argFormulaIds)
      {
        var argFormulaItem = argFormulas.First(f => f.Id == argFormulaId);
        var argFormulaStrings = argFormulaItem.FormulaStrings;
        foreach (var argFormulaString in argFormulaStrings)
        {
          // 量化する場合、対象の自由変数を束縛変数に変換する。
          if (isQuant && argFormulaString.SymbolId == boundId)
          {
            if (boundVariable == null)
            {
              throw new InvalidDataException("Not Found bound variable. Register bound variable symbol");
            }
            strings.Add(
              new FormulaString
              {
                SerialNo = serialNo++,
                SymbolId = boundVariable.Id
              }
            );
          }
          // 量化しない場合
          else
          {
            strings.Add(
              new FormulaString
              {
                SerialNo = serialNo++,
                SymbolId = argFormulaString.SymbolId
              }
            );
          }
        }
      }
      return strings;
    }

    /// <summary>
    /// POSTパラメータからFormulaChainを作成
    /// </summary>
    /// <param name="argFormulaIds">POSTで渡された論理式IDのリスト</param>
    /// <param name="argFormulas">引数論理式のオブジェクトリスト</param>
    /// <param name="isQuant">一文字目が量化記号かどうか</param>
    /// <param name="boundId">束縛する自由変数のFormulaID</param>
    /// <returns></returns>
    private static List<FormulaChain> CreateFormulaChainFromPostParam(
      List<long> argFormulaIds,
      List<Formula> argFormulas,
      bool isQuant,
      long? boundId
    )
    {
      // 戻り値
      var ret = new List<FormulaChain>();
      // 新規FormulaChainのserialNo
      long serialNo = 0;
      // カウント中の文字の位置。一文字目が量化記号の場合に新たに追加するChainを作成するために使用する
      long symbolCount = 0;
      // 論理式の長さの和
      long formulaLength = 1;
      foreach (var argFormulaId in argFormulaIds)
      {
        var argFormula = argFormulas.First(f => f.Id == argFormulaId);
        // 一文字目が量化記号の場合、新たなChainを追加する
        if (isQuant)
        {
          foreach (var argFormulaString in argFormula.FormulaStrings)
          {
            symbolCount++;
            if (argFormulaString.SymbolId != boundId) continue;
            ret.Add(new FormulaChain
            {
              SerialNo = serialNo++,
              FromFormulaStringSerialNo = 0,
              ToFormulaStringSerialNo = symbolCount
            });
          }
        }

        // 引数の論理式のChainは、From/Toをそれぞれ直前までの文字列の長さを足して作成する
        foreach (var argFormulaChain in argFormula.FormulaChains ?? new List<FormulaChain>())
        {
          ret.Add(new FormulaChain
          {
            SerialNo = serialNo++,
            FromFormulaStringSerialNo = argFormulaChain.FromFormulaStringSerialNo + formulaLength,
            ToFormulaStringSerialNo = argFormulaChain.ToFormulaStringSerialNo + formulaLength
          });
        }
        formulaLength += argFormula.Length;
      }

      return ret;
    }

    /// <summary>
    /// POSTパラメータからFormulaオブジェクトを作成
    /// </summary>
    /// <param name="postParam">POSTパラメータ</param>
    /// <returns>Formulaオブジェクト</returns>
    /// <exception cref="InvalidDataException"></exception>
    private async Task<Formula> CreateFormulaFromPostParam(PostParameter postParam)
    {
      // 一文字目を取得
      var firstSymbol = await _context.Symbols.FindAsync(postParam.FirstSymbolId)
                        ?? throw new InvalidDataException("first symbol is not found");

      // 一文字目が量化記号かどうかを取得
      var isQuant = firstSymbol.SymbolTypeId == (long)Const.SymbolType.TermQuantifier
                 || firstSymbol.SymbolTypeId == (long)Const.SymbolType.PropositionQuantifier;

      // 一文字目が量化記号のとき、束縛変数を取得
      Symbol? boundVariable = null;
      if (isQuant)
      {
        boundVariable = await _context.Symbols.FirstAsync(s => s.SymbolTypeId == (long)Const.SymbolType.BoundVariable)
                        ?? throw new InvalidDataException("bound variable is not found");
      }

      // 引数の論理式を取得
      var argFormulas = new List<Formula>();
      if (postParam.ArgumentedFormulaIds.Any())
      {
        argFormulas = await _context.Formulas
          .Where(f => postParam.ArgumentedFormulaIds.Contains(f.Id))
          .ToListAsync();
        var argStrings = await _context.FormulaStrings
          .Where(f => postParam.ArgumentedFormulaIds.Contains(f.FormulaId))
          .ToListAsync();
        var argChains = await _context.FormulaChains
          .Where(f => postParam.ArgumentedFormulaIds.Contains(f.FormulaId))
          .ToListAsync();
        var symbolTypes = await _context.SymbolTypes.ToListAsync();
        foreach (var formula in argFormulas)
        {
          formula.FormulaStrings = argStrings.Where(s => s.FormulaId == formula.Id).OrderBy(s => s.SerialNo).ToList();
          foreach (var fs in formula.FormulaStrings)
          {
            _context.Entry(fs).State = EntityState.Unchanged;
            _context.Entry(fs.Symbol).State = EntityState.Unchanged;
          }
          formula.FormulaChains = argChains.Where(s => s.FormulaId == formula.Id).OrderBy(s => s.SerialNo).ToList();
          foreach (var fc in formula.FormulaChains)
          {
            _context.Entry(fc).State = EntityState.Unchanged;
          }
        }
      }

      // 論理式文字列を構成
      var strings = CreateFormulaStringFromPostParam(
        firstSymbol,
        postParam.ArgumentedFormulaIds,
        argFormulas,
        isQuant,
        boundVariable,
        postParam.BoundVariableId
      );

      // 論理式鎖を構成
      var chains = CreateFormulaChainFromPostParam(
        postParam.ArgumentedFormulaIds,
        argFormulas,
        isQuant,
        postParam.BoundVariableId
      );

      // 戻り値作成
      var ret = new Formula
      {
        Meaning = postParam.Meaning,
        FormulaStrings = strings,
        FormulaChains = chains
      };

      // バリデーション
      var valid = PostValidation(ret, argFormulas);
      if (valid != null)
      {
        throw new ArgumentException(valid);
      }

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

    /// <summary>
    /// POST時のバリデーション
    /// </summary>
    /// <param name="formula">POSTパラメータから作成したFormulaオブジェクト</param>
    /// <param name="argFormulas">引数にとる論理式オブジェクト</param>
    /// <returns>エラーなしならnull。エラーありならメッセージ</returns>
    private static string? PostValidation(Formula formula, List<Formula> argFormulas)
    {
      var firstSymbol = formula.FormulaStrings[0].Symbol;
      if (firstSymbol == null)
      {
        return "first symbol is not found";
      }

      if (firstSymbol.Arity != argFormulas.Count)
      {
        return "Invalid Argument Formula Count";
      }

      int argCnt = 0;
      foreach (var argFormula in argFormulas)
      {
        argCnt++;
        if (argFormula.FormulaTypeId != firstSymbol.ArityFormulaTypeId)
        {
          return $"Invalid Argument Formula Type on #{argCnt}";
        }
      }
      return null;
    }
  }

  /// <summary>
  /// POSTパラメータ
  /// </summary>
  public class PostParameter
  {
    public long FirstSymbolId { get; set; }
    public List<long> ArgumentedFormulaIds { get; set; } = new List<long>();
    public long? BoundVariableId { get; set; }
    public string? Meaning { get; set; }
  }
}
