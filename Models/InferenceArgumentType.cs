namespace MathApi.Models;

public class InferenceArgumentType
{
  public int Id { get; set; }
  public string Name { get; set; } = "";

  public List<InferenceArgument>? InferenceArguments { get; }
}