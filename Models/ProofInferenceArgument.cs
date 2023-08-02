using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(TheoremId), nameof(ProofSerialNo), nameof(ProofInferenceSerialNo), nameof(SerialNo))]
public class ProofInferenceArgument
{
  public ProofInference ProofInference { get; } = new();
  public long TheoremId { get; set; }
  public long ProofSerialNo { get; set; }
  public long ProofInferenceSerialNo { get; set; }
  public long SerialNo { get; set; }
  public AxiomProposition? AxiomProposition { get; }
  public long? AxiomId { get; set; }
  public Formula Formula { get; } = new();
  public long FormulaId { get; set; }
}
