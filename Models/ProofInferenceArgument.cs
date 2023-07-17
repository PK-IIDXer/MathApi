using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(ProofId), nameof(ProofSerialNo), nameof(ProofInferenceSerialNo), nameof(SerialNo))]
public class ProofInferenceArgument
{
  public ProofInference ProofInference { get; } = new();
  public long ProofId { get; set; }
  public long ProofSerialNo { get; set; }
  public long ProofInferenceSerialNo { get; set; }
  public long SerialNo { get; set; }
  public AxiomProposition? AxiomProposition { get; }
  public long? AxiomId { get; set; }
  public long? AxiomPropositionSerialNo { get; set; }
  public TheoremAssumption? TheoremAssumption { get; }
  public long? TheoremAssumptionTheoremId { get; set; }
  public long? TheoremAssumptionSerialNo { get; set; }
  public Formula Formula { get; } = new();
  public long FormulaId { get; set; }
}
