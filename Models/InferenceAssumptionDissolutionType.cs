namespace MathApi.Models;

/// <summary>
/// 推論規則仮定解消種類
/// </summary>
/// <remarks>
/// 無し・必須・任意の３種類
/// </remarks>
public class InferenceAssumptionDissolutionType
{
  /// <summary>
  /// 推論規則仮定解消種類ID
  /// </summary>
  public int Id { get; set; }
  /// <summary>
  /// 推論規則仮定解消種類名称
  /// </summary>
  public string Name { get; set; } = "";

  public List<InferenceAssumption>? InferenceAssumptions { get; }
}
