using MathApi.Models;

namespace MathApi.Controllers;

public class InferenceDto
{
  public long Id { get; set; }
  public string Name { get; set; } = "";
  public bool IsAssumptionAdd { get; set; } = false;
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
      public int DestinationArgumentSerialNo { get; set; }
      public bool IsConstraintPredissolvedAssumption { get; set; }
    }
  }

  public class AssumptionDto
  {
    public int SerialNo { get; set; }
    public int FormulaStructId { get; set; } = new();
    public List<ArgumentMappingDto> FormulaStructArgumentMappings { get; set; } = new();
    public DissolutableDto? DissolutableAssumption { get; set; }
    public class DissolutableDto
    {
      public int FormulaStructId { get; set; }
      public List<ArgumentMappingDto> FormulaStructArgumentMappings { get; set; } = new();
      public bool IsForce { get; set; }
    }
  }

  public class ConclusionDto
  {
    public int FormulaStructId { get; set; }
    public List<ArgumentMappingDto> FormulaStructArgumentMappings { get; set; } = new();
    public bool AddAssumption { get; set; } = false;
  }

  public class ArgumentMappingDto
  {
    public int SerialNo { get; set; }
    public int FormulaStructArgumentSerialNo { get; set; }
    public int InferenceArgumentSerialNo { get; set; }
  }

  public Inference CreateModel()
  {
    return new Inference
    {
      Name = Name,
      IsAssumptionAdd = IsAssumptionAdd,
      Arguments = Arguments.Select(a => new InferenceArgument
      {
        SerialNo = a.SerialNo,
        FormulaLabelId = a.FormulaLabelId,
        InferenceArgumentConstraints = a.Constraints?.Select(c => new InferenceArgumentConstraint
        {
          InferenceArgumentSerialNo = a.SerialNo,
          SerialNo = c.SerialNo,
          ConstraintDestinationInferenceArgumentSerialNo = c.DestinationArgumentSerialNo,
          IsConstraintPredissolvedAssumption = c.IsConstraintPredissolvedAssumption
        }).ToList() ?? new()
      }).ToList(),
      Assumptions = Assumptions.Select(a => new InferenceAssumption
      {
        SerialNo = a.SerialNo,
        FormulaStructId = a.FormulaStructId,
        FormulaStructArgumentMappings = a.FormulaStructArgumentMappings.Select(m => new InferenceFormulaStructArgumentMapping
        {
          SerialNo = m.SerialNo,
          FormulaStructId = a.FormulaStructId,
          FormulaStructArgumentSerialNo = m.FormulaStructArgumentSerialNo,
          InferenceArgumentSerialNo = m.InferenceArgumentSerialNo
        }).ToList()
      }).ToList(),
      Conclusions = new List<InferenceConclusion> { new() {
        FormulaStructId = Conclusion.FormulaStructId,
        FormulaStructArgumentMappings = Conclusion.FormulaStructArgumentMappings.Select(m => new InferenceFormulaStructArgumentMapping {
          SerialNo = m.SerialNo,
          FormulaStructId = Conclusion.FormulaStructId,
          FormulaStructArgumentSerialNo = m.FormulaStructArgumentSerialNo,
          InferenceArgumentSerialNo = m.InferenceArgumentSerialNo
        }).ToList()
      }}
    };
  }
}

public class DefineSymbolDto
{
  public long SymbolId { get; set; }
  public long FormulaId { get; set; }
  public List<long>? ArgumentSymbolIds { get; set; }
}
