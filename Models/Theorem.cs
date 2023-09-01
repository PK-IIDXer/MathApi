using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MathApi.Models;

public class Theorem
{
  public long Id { get; set; }
  public string Name { get; set; } = "";
  public bool IsInference { get; set; } = false;
  public bool IsProved { get; set; } = false;

  public List<TheoremAssumption> TheoremAssumptions { get; set; } = new();
  public List<TheoremConclusion> TheoremConclusions { get; set; } = new();

  public Inference? Inference { get; set; }
  public int? InferenceId { get; set; }

  public List<Proof>? Proofs { get; } = new();
}
