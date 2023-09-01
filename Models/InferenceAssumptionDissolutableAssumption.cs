using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

/// <summary>
/// 推論規則解消可能仮定
/// </summary>
[PrimaryKey(nameof(InferenceId), nameof(InferenceAssumptionSerialNo))]
public class InferenceAssumptionDissolutableAssumption
{
  public InferenceAssumption InferenceAssumption { get; } = new();
  /// <summary>
  /// 推論規則ID
  /// </summary>
  public long InferenceId { get; set; }
  /// <summary>
  /// 推論規則仮定連番
  /// </summary>
  public int InferenceAssumptionSerialNo { get; set; }
  public FormulaStruct? FormulaStruct { get; set; }
  /// <summary>
  /// 論理式構成ID
  /// </summary>
  public int FormulaStructId { get; set; }
  public List<InferenceFormulaStructArgumentMapping> FormulaStructArgumentMappings { get; set; } = new();
  /// <summary>
  /// 論理式構成-引数マッピング
  /// </summary>
  public int FormulaStructArgumentMappingSerialNo { get; set;}
  /// <summary>
  /// 解消が強制か否か
  /// </summary>
  public bool IsForce { get; set; }
}