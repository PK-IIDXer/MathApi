using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

/// <summary>
/// 証明仮定
/// </summary>
/// <remarks>
/// 証明において未解消の仮定が存在するかどうかを確認するためのテーブル
/// </remarks>
[PrimaryKey(nameof(TheoremId), nameof(ProofSerialNo), nameof(SerialNo))]
public class ProofAssumption
{
  public Proof Proof { get; } = new();
  /// <summary>
  /// 定理ID
  /// </summary>
  public long TheoremId { get; set; }
  /// <summary>
  /// 証明連番
  /// </summary>
  public long ProofSerialNo { get; set; }
  /// <summary>
  /// 証明仮定連番
  /// </summary>
  public long SerialNo { get; set; }
  public ProofInference? AddedProofInference { get; set; }
  /// <summary>
  /// 証明仮定を追加した証明推論連番
  /// </summary>
  public long AddedProofInferenceSerialNo { get; set; }
  public ProofInference? DissolutedProofInference { get; }
  /// <summary>
  /// 当該証明仮定を解消した証明推論連番
  /// </summary>
  public long? DissolutedProofInferenceSerialNo { get; set; }
}
