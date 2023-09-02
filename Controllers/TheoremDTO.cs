using MathApi.Models;

namespace MathApi.Controllers;

public class TheoremDto
{
  public long Id { get; set; }
  public string Name { get; set; } = "";
  public List<TheoremPropositionDto>? Assumptions { get; set; }
  public TheoremPropositionDto? Conclusion { get; set; }
  public InferenceDto? Inference { get; set; }
  public Theorem CreateModel()
  {
    var ret =  new Theorem
    {
      Id = Id,
      Name = Name,
      IsProved = false
    };

    if (Inference != null)
    {
      if (Assumptions != null && Assumptions.Count > 0)
        throw new ArgumentException("Use inference if Inference is not null");
      if (Conclusion != null)
        throw new ArgumentException("Use inference if Inference is not null");
      ret.Inference = Inference.CreateModel();
      ret.Inference.TheoremId = Id;
    }
    else
    {
      if (Inference != null)
        throw new ArgumentException("Can't Input inference if not IsInference");
      if (Conclusion == null)
        throw new ArgumentException("Input Conclusion if not IsInference");
      ret.TheoremAssumptions = Assumptions?.Select(a => new TheoremAssumption{
        TheoremId = Id,
        SerialNo = a.SerialNo,
        FormulaId = a.FormulaId
      }).ToList() ?? new List<TheoremAssumption>();
      ret.TheoremConclusions = new List<TheoremConclusion> {
        new() {
          TheoremId = Id,
          SerialNo = Conclusion.SerialNo,
          FormulaId = Conclusion.FormulaId
        }
      };
    }

    return ret;
  }

  public class TheoremPropositionDto
  {
    public long SerialNo { get; set; }
    public long FormulaId { get; set; }
  }
}