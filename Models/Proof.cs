using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(ProofHeadId), nameof(ProofHeadSerialNo), nameof(SerialNo))]
public class Proof
{
  public ProofHead ProofHead { get; } = new();
  public long ProofHeadId { get; set; }
  public long ProofHeadSerialNo { get; set; }
  public long SerialNo { get; set; }
  public Inference Inference { get; } = new();
  public long InferenceId { get; set; }
  public long NextProofSerialNo { get; set; }
  public Formula ConclusionFormula { get; } = new();
  public long ConclusionFormulaId { get; set; }

  public List<ProofAssumption>? ProofAssumptions { get; set; }
  public List<ProofArgument>? ProofArguments { get; set; }
}
