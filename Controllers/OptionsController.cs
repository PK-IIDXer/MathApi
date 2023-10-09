using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MathApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MathApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OptionsController : ControllerBase
  {
    private readonly MathDbContext _context;

    public OptionsController(MathDbContext context)
    {
      _context = context;
    }

    [HttpGet("FormulaTypes")]
    public async Task<ActionResult<IEnumerable<FormulaType>>> GetFormulaTypes()
    {
      return await _context.FormulaTypes.OrderBy(ft => ft.Id).ToListAsync();
    }

    [HttpGet("FormulaLabelTypes")]
    public async Task<ActionResult<IEnumerable<FormulaLabelType>>> GetFormulaLabelTypes()
    {
      return await _context.FormulaLabelTypes.OrderBy(ft => ft.Id).ToListAsync();
    }

    [HttpGet("SymbolTypes")]
    public async Task<ActionResult<IEnumerable<SymbolType>>> GetSymbolTypes()
    {
      return await _context.SymbolTypes.OrderBy(ft => ft.Id).ToListAsync();
    }
  }
}
