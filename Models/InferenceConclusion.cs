using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace MathApi.Models;

/// <summary>
/// 推論規則仮定
/// </summary>
[PrimaryKey(nameof(InferenceId))]
public class InferenceConclusion
{
  [JsonIgnore]
  public Inference Inference { get; set; } = new();
  /// <summary>
  /// 推論規則ID
  /// </summary>
  public long InferenceId { get; set; }
  public FormulaStruct? FormulaStruct { get; set; }
  /// <summary>
  /// 論理式構成ID
  /// </summary>
  public long FormulaStructId { get; set; }
  public List<InferenceFormulaStructArgumentMapping> FormulaStructArgumentMappings { get; set; } = new();
  /// <summary>
  /// 論理式構成-引数マッピング
  /// </summary>
  public int FormulaStructArgumentMappingSerialNo { get; set;}
  /// <summary>
  /// 仮定追加有無
  /// </summary>
  public bool AddAssumption { get; set; } = false;
}
