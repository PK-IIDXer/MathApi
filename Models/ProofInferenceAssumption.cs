using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(ProofId), nameof(ProofSerialNo), nameof(ProofInferenceSerialNo))]
public class ProofInferenceAssumption
{
  public ProofInference ProofInference { get; } = new();
  public long ProofId { get; set; }
  public long ProofSerialNo { get; set; }
  public long ProofInferenceSerialNo { get; set; }
  public Formula Formula { get; } = new();
  public long FormulaId { get; set; }
  public ProofInference? DissolutedProofInference { get; }
  public long? DissolutedProofInferenceSerialNo { get; set; }
}
