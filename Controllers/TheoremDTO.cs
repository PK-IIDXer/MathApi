using MathApi.Models;

namespace MathApi.Controllers;

public class TheoremDto
{
  public long Id { get; set; }
  public string Name { get; set; } = "";
  public bool IsInference { get; set; } = false;
  public List<TheoremPropositionDto>? Assumptions { get; set; }
  public TheoremPropositionDto? Conclusion { get; set; }
  public InferenceDto? Inference { get; set; }
  public Theorem CreateModel()
  {
    var ret =  new Theorem
    {
      Id = Id,
      Name = Name,
      IsInference = IsInference,
      IsProved = false
    };

    if (IsInference)
    {
      if (Inference == null)
        throw new ArgumentException("Input inference if IsInference");
      if (Conclusion != null)
        throw new ArgumentException("Use inference if IsInference");
      ret.Inference = Inference.CreateModel();
      if (!ret.Inference.InferenceArguments.Any(ia => ia.InferenceArgumentTypeId == (int)Const.InferenceArgumentType.Proposition))
        throw new ArgumentException("Should not use inference if there is no propositional argument");
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
        new TheoremConclusion
        {
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