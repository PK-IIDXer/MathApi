using MathApi.Models;

namespace MathApi.Controllers;

public class AxiomDto
{
  public long Id { get; set; }
  public string Name { get; set; } = "";
  public string? Remarks { get; set; }
  public List<Proposition> Propositions { get; set; } = new();

  public class Proposition
  {
    public long SerialNo { get; set; }
    public long FormulaId { get; set; }
    public string? Remarks { get; set; }
  }

  public Axiom CreateModel()
  {
    return new Axiom
    {
      Id = Id,
      Name = Name,
      Remarks = Remarks,
      AxiomPropositions = Propositions.Select(p => new AxiomProposition
      {
        AxiomId = Id,
        SerialNo = p.SerialNo,
        FormulaId = p.FormulaId,
        Meaning = p.Remarks
      }).ToList()
    };
  }
}
