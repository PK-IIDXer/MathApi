using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MathApi.Models;

public class Theorem
{
  public long Id { get; set; }
  public string Name { get; set; } = "";
  public string Meaning { get; set; } = "";
  public bool IsProved { get; set; } = false;

  public List<TheoremAssumption> Assumptions { get; set; } = new();
  public List<TheoremConclusion> Conclusions { get; set; } = new();

  public Inference? Inference { get; set; }
  public long? InferenceId { get; set; }

  public List<Proof>? Proofs { get; } = new();
}
