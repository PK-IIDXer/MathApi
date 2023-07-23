using MathApi.Models;

namespace MathApi.Controllers;

public class TheoremDto
{
  public long Id { get; set; }
  public string Name { get; set; } = "";
  public bool IsProved { get; set; } = false;
  public List<TheoremPropositionDto> Assumptions { get; set; } = new();
  public TheoremPropositionDto Conclusion { get; set; } = new();
  public Theorem CreateModel()
  {
    return new Theorem
    {
      Id = Id,
      Name = Name,
      IsProved = IsProved,
      TheoremAssumptions = Assumptions.Select(a => new TheoremAssumption{
        TheoremId = Id,
        SerialNo = a.SerialNo,
        FormulaId = a.FormulaId
      }).ToList(),
      TheoremConclusions = new List<TheoremConclusion> {
        new TheoremConclusion
        {
          TheoremId = Id,
          SerialNo = Conclusion.SerialNo,
          FormulaId = Conclusion.FormulaId
        }
      }
    };
  }

  public class TheoremPropositionDto
  {
    public long SerialNo { get; set; }
    public long FormulaId { get; set; }
  }
}