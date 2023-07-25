using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(TheoremId), nameof(ProofSerialNo), nameof(SerialNo))]
public class ProofAssumption
{
  public Proof Proof { get; } = new();
  public long TheoremId { get; set; }
  public long ProofSerialNo { get; set; }
  public long SerialNo { get; set; }
  public Formula Formula { get; } = new();
  public long FormulaId { get; set; }
  public ProofInference AddedProofInference { get; set; } = new();
  public long AddedProofInferenceSerialNo { get; set; }
  public ProofInference LastUsedProofInference { get; set; } = new();
  public long LastUsedProofInferenceSerialNo { get; set; }
  public ProofInference? DissolutedProofInference { get; }
  public long? DissolutedProofInferenceSerialNo { get; set; }
}
