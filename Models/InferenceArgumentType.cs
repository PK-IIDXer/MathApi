namespace MathApi.Models;

/// <summary>
/// 推論規則引数種類
/// </summary>
/// <remarks>
/// 項、命題、自由変数の３種類
/// </remarks>
public class InferenceArgumentType
{
  /// <summary>
  /// 推論規則引数種類ID
  /// </summary>
  public int Id { get; set; }
  /// <summary>
  /// 推論規則引数種類名称
  /// </summary>
  public string Name { get; set; } = "";

  public List<InferenceArgument>? InferenceArguments { get; }
}