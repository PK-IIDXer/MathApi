using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(TheoremId), nameof(ProofSerialNo), nameof(SerialNo))]
public class ProofInference
{
  public Proof Proof { get; } = new();
  public long TheoremId { get; set; }
  public long ProofSerialNo { get; set; }
  public long SerialNo { get; set; }
  public Inference Inference { get; } = new();
  public long InferenceId { get; set; }
  public Formula ConclusionFormula { get; } = new();
  public long ConclusionFormulaId { get; set; }

  public List<ProofInference>? PreviousProofInferences { get; set; }
  public ProofInference? NextProofInference { get; set; }
  public long? NextProofInferenceSerialNo { get; set; }

  public List<ProofInferenceArgument>? ProofInferenceArguments { get; set; }

  public ProofAssumption? AddingProofInference { get; }
  public ProofAssumption? LastUsingProofInference { get; }
  public ProofAssumption? DissolutingAssumption { get; }
}
