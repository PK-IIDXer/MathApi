using MathApi.Models;

namespace MathApi.Controllers;

public class InferenceDto
{
  public long Id { get; set; }
  public string Name { get; set; } = "";
  public bool IsAssumptionAdd { get; set; } = false;
  public List<ArgumentDto>? Arguments { get; set; } = new();
  public List<AssumptionDto>? Assumptions { get; set; }
  public List<InferenceFormulaDto> Conclusions { get; set; } = new(); 

  public class ArgumentDto
  {
    public int SerialNo { get; set; }
    public int InferenceArgumentTypeId { get; set; }
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
    public int DissolutionTypeId { get; set; }
    public List<InferenceFormulaDto> Formulas { get; set; } = new();
    public List<InferenceFormulaDto>? DissolutableAssumptionFormulas { get; set; }
  }

  public class InferenceFormulaDto
  {
    public int SerialNo { get; set; }
    public long? SymbolId { get; set; }
    public int? BoundArgumentSerialNo { get; set; }
    public int? ArgumentSerialNo { get; set; }
    public int? SubstitutionArgumentFromSerialNo { get; set; }
    public int? SubstitutionArgumentToSerialNo { get; set; }
  }

  public Inference CreateModel()
  {
    return new Inference
    {
      Id = Id,
      Name = Name,
      IsAssumptionAdd = IsAssumptionAdd,
      InferenceArguments = Arguments?.Select(a => new InferenceArgument
      {
        InferenceId = Id,
        SerialNo = a.SerialNo,
        InferenceArgumentTypeId = a.InferenceArgumentTypeId,
        InferenceArgumentConstraints = a.Constraints?.Select(c => new InferenceArgumentConstraint
        {
          InferenceId = Id,
          InferenceArgumentSerialNo = a.SerialNo,
          SerialNo = c.SerialNo,
          ConstraintDestinationInferenceArgumentSerialNo = c.DestinationArgumentSerialNo,
          IsConstraintPredissolvedAssumption = c.IsConstraintPredissolvedAssumption
        }).ToList()
      }).ToList(),
      InferenceAssumptions = Assumptions?.Select(a => new InferenceAssumption
      {
        InferenceId = Id,
        SerialNo = a.SerialNo,
        InferenceAssumptionDissolutionTypeId = a.DissolutionTypeId,
        InferenceAssumptionFormulas = a.Formulas.Select(f => new InferenceAssumptionFormula
        {
          InferenceId = Id,
          InferenceAssumptionSerialNo = a.SerialNo,
          SerialNo = f.SerialNo,
          SymbolId = f.SymbolId,
          BoundInferenceArgumentSerialNo = f.BoundArgumentSerialNo,
          InferenceArgumentSerialNo = f.ArgumentSerialNo,
          SubstitutionInferenceArgumentFromSerialNo = f.SubstitutionArgumentFromSerialNo,
          SubstitutionInferenceArgumentToSerialNo = f.SubstitutionArgumentToSerialNo
        }).ToList(),
        InferenceAssumptionDissolutableAssumptionFormulas = a.DissolutableAssumptionFormulas?.Select(f => new InferenceAssumptionDissolutableAssumptionFormula
        {
          InferenceId = Id,
          InferenceAssumptionSerialNo = a.SerialNo,
          SerialNo = f.SerialNo,
          SymbolId = f.SymbolId,
          BoundInferenceArgumentSerialNo = f.BoundArgumentSerialNo,
          InferenceArgumentSerialNo = f.ArgumentSerialNo,
          SubstitutionInferenceArgumentFromSerialNo = f.SubstitutionArgumentFromSerialNo,
          SubstitutionInferenceArgumentToSerialNo = f.SubstitutionArgumentToSerialNo
        }).ToList()
      }).ToList(),
      InferenceConclusionFormulas = Conclusions.Select(f => new InferenceConclusionFormula
      {
        InferenceId = Id,
        SerialNo = f.SerialNo,
        SymbolId = f.SymbolId,
        BoundInferenceArgumentSerialNo = f.BoundArgumentSerialNo,
        InferenceArgumentSerialNo = f.ArgumentSerialNo,
        SubstitutionInferenceArgumentFromSerialNo = f.SubstitutionArgumentFromSerialNo,
        SubstitutionInferenceArgumentToSerialNo = f.SubstitutionArgumentToSerialNo
      }).ToList()
    };
  }
}

public class DefineSymbolDto
{
  public long SymbolId { get; set; }
  public long FormulaId { get; set; }
  public List<long>? ArgumentSymbolIds { get; set; }
}
