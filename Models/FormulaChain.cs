using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(FormulaId), nameof(SerialNo))]
public class FormulaChain
{
  public Formula? Formula { get; set; }
  public long FormulaId { get; set; }
  public long SerialNo { get; set; }
  public FormulaString? FromFormulaString { get; set; }
  public long FromFormulaStringSerialNo { get; set; }
  public FormulaString? ToFormulaString { get; set; }
  public long ToFormulaStringSerialNo { get; set; }
}