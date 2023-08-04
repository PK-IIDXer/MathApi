using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

/// <summary>
/// 証明
/// </summary>
[PrimaryKey(nameof(TheoremId), nameof(SerialNo))]
public class Proof
{
  public Theorem Theorem { get; } = new();
  /// <summary>
  /// 定理ID
  /// </summary>
  public long TheoremId { get; set; }
  /// <summary>
  /// 証明連番
  /// </summary>
  public long SerialNo { get; set; }

  public List<ProofInference> ProofInferences { get; set; } = new();
  public List<ProofAssumption> ProofAssumptions { get; set; } = new();
}
