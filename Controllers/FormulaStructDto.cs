using MathApi.Models;

namespace MathApi.Controllers;

public class FormulaStructDto
{
  public long Id { get; set; }
  public string? Meaning { get; set; }
  public List<FormulaStructArgumentDto> Arguments { get; set; } = new();
  public List<FormulaStructStringDto> Strings { get; set; } = new();

  public class FormulaStructArgumentDto
  {
    public int SerialNo { get; set; }
    public int FormulaLabelId { get; set; }
  }
  public class FormulaStructStringDto
  {
    public int SerialNo { get; set; }
    public long? SymbolId { get; set; }
    public int? BoundArgumentSerialNo { get; set; }
    public int? ArgumentSerialNo { get; set; }
    public List<FormulaStructStringSubstitutionDto> Substitutions { get; set; } = new();

    public class FormulaStructStringSubstitutionDto
    {
      public int SerialNo { get; set; }
      public int ArgumentFromSerialNo { get; set; }
      public int ArgumentToSerialNo { get; set; }
    }
  }

  public FormulaStruct CreateModel()
  {
    return new FormulaStruct
    {
      Id = Id,
      Meaning = Meaning,
      Arguments = Arguments.OrderBy(a => a.SerialNo).Select(a => new FormulaStructArgument
      {
        FormulaStructId = Id,
        SerialNo = a.SerialNo,
        LabelId = a.FormulaLabelId
      }).ToList(),
      Strings = Strings.OrderBy(s => s.SerialNo).Select(s => new FormulaStructString
      {
        FormulaStructId = Id,
        SerialNo = s.SerialNo,
        SymbolId = s.SymbolId,
        BoundArgumentSerialNo = s.BoundArgumentSerialNo,
        ArgumentSerialNo = s.ArgumentSerialNo,
        Substitutions = s.Substitutions.OrderBy(ss => ss.SerialNo).Select(sb => new FormulaStructStringSubstitution
        {
          FormulaStructId = Id,
          FormulaStructStringSerialNo = s.SerialNo,
          SerialNo = sb.SerialNo,
          ArgumentFromSerialNo = sb.ArgumentFromSerialNo,
          ArgumentToSerialNo = sb.ArgumentToSerialNo
        }).ToList()
      }).ToList()
    };
  }
}
