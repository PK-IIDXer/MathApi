using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

/// <summary>
/// 推論規則仮定
/// </summary>
[PrimaryKey(nameof(InferenceId), nameof(SerialNo))]
public class InferenceAssumption
{
  public Inference Inference { get; set; } = new();
  /// <summary>
  /// 推論規則ID
  /// </summary>
  public long InferenceId { get; set; }
  /// <summary>
  /// 推論規則仮定連番
  /// </summary>
  public int SerialNo { get; set; }
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

  public InferenceAssumptionDissolutableAssumption? DissolutableAssumption { get; set; }
}
