using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(FormulaId), nameof(SerialNo))]
public class FormulaString
{
  public Formula? Formula { get; set; }
  [Required]
  public long FormulaId { get; set; }
  [Required]
  public long SerialNo { get; set; }
  public Symbol? Symbol { get; set; }
  [Required]
  public long SymbolId { get; set; }
}
