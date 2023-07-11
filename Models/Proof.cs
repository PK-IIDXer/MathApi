using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(TheoremId), nameof(ProofHeadSerialNo), nameof(SerialNo))]
public class Proof
{
  public ProofHead ProofHead { get; } = new();
  public long TheoremId { get; set; }
  public long ProofHeadSerialNo { get; set; }
  public long SerialNo { get; set; }
  public Inference Inference { get; } = new();
  public long InferenceId { get; set; }
  public long NextProofSerialNo { get; set; }
  public Formula ConclusionFormula { get; } = new();
  public long ConclusionFormulaId { get; set; }

  public List<ProofAssumption>? ProofAssumptions { get; }
  public List<ProofArgument>? ProofArguments { get; }
}
