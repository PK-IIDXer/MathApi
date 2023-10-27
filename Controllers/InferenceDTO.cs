using MathApi.Models;

namespace MathApi.Controllers;

public class InferenceDto
{
  public long Id { get; set; }
  public string Name { get; set; } = "";
  public List<ArgumentDto> Arguments { get; set; } = new();
  public List<AssumptionDto> Assumptions { get; set; } = new();
  public ConclusionDto Conclusion { get; set; } = new();

  public class ArgumentDto
  {
    public int SerialNo { get; set; }
    public int FormulaLabelId { get; set; }
    public List<ConstraintDto>? Constraints { get; set; }

    public class ConstraintDto
    {
      public int SerialNo { get; set; }
      public int? DestinationArgumentSerialNo { get; set; }
      public int? AssumptionSerialNoForConstraintToAllPredissolvedAssumption { get; set; }
    }
  }

  public class AssumptionDto
  {
    public int SerialNo { get; set; }
    public int FormulaStructId { get; set; } = new();
    public DissolutableDto? DissolutableAssumption { get; set; }
    public class DissolutableDto
    {
      public int FormulaStructId { get; set; }
      public bool IsForce { get; set; }
    }
  }

  public class ConclusionDto
  {
    public int FormulaStructId { get; set; }
    public bool AddAssumption { get; set; } = false;
  }

  public Inference CreateModel()
  {
    return new Inference
    {
      Id = Id,
      Name = Name,
      Arguments = Arguments.Select(a => new InferenceArgument
      {
        InferenceId = Id,
        SerialNo = a.SerialNo,
        FormulaLabelId = a.FormulaLabelId,
        InferenceArgumentConstraints = a.Constraints?.Select(c => new InferenceArgumentConstraint
        {
          InferenceId = Id,
          InferenceArgumentSerialNo = a.SerialNo,
          SerialNo = c.SerialNo,
          ConstraintDestinationInferenceArgumentSerialNo = c.DestinationArgumentSerialNo,
          AssumptionSerialNoForConstraintToAllPredissolvedAssumption = c.AssumptionSerialNoForConstraintToAllPredissolvedAssumption
        }).ToList() ?? new()
      }).ToList(),
      Assumptions = Assumptions.Select(a => new InferenceAssumption
      {
        InferenceId = Id,
        SerialNo = a.SerialNo,
        FormulaStructId = a.FormulaStructId,
        DissolutableAssumption = a.DissolutableAssumption == null ? null : new InferenceAssumptionDissolutableAssumption
        {
          InferenceId = Id,
          InferenceAssumptionSerialNo = a.SerialNo,
          FormulaStructId = a.DissolutableAssumption.FormulaStructId,
          IsForce = a.DissolutableAssumption.IsForce
        }
      }).ToList(),
      Conclusions = new List<InferenceConclusion> { new() {
        InferenceId = Id,
        FormulaStructId = Conclusion.FormulaStructId,
        AddAssumption = Conclusion.AddAssumption
      }}
    };
  }
}
