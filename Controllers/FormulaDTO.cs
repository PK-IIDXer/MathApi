using MathApi.Models;

namespace MathApi.Controllers;

public class FormulaDto
{
  public long Id { get; set; }
  public string? Meaning { get; set; }
  public List<FormulaStringDto> Strings { get; set; } = new();
  public List<FormulaChainDto> Chains { get; set; } = new();

  public class FormulaStringDto
  {
    public long SerialNo { get; set; }
    public long SymbolId { get; set; }
  }

  public class FormulaChainDto
  {
    public long SerialNo { get; set; }
    public long FromStringSerialNo { get; set; }
    public long ToStringSerialNo { get; set; }
  }

  public Formula CreateModel()
  {
    return new Formula
    {
      Id = Id,
      Meaning = Meaning,
      Strings = Strings.OrderBy(s => s.SerialNo).Select(s => new FormulaString
      {
        FormulaId = Id,
        SerialNo = s.SerialNo,
        SymbolId = s.SymbolId
      }).ToList(),
      Chains = Chains.OrderBy(c => c.SerialNo).Select(c => new FormulaChain
      {
        FormulaId = Id,
        SerialNo = c.SerialNo,
        FromFormulaStringSerialNo = c.FromStringSerialNo,
        ToFormulaStringSerialNo = c.ToStringSerialNo
      }).ToList()
    };
  }
}