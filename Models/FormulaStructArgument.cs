using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;
using System.Text.Json.Serialization;

[PrimaryKey(nameof(FormulaStructId), nameof(SerialNo))]
public class FormulaStructArgument
{
  public FormulaStruct FormulaStruct { get; set; } = new();
  public long FormulaStructId { get; set; }
  public int SerialNo { get; set; }
  public FormulaLabel? Label { set; get; }
  public int LabelId { get; set; }

  [JsonIgnore]
  public List<FormulaStructString> StringsToBoundArgument { get; } = new();
  [JsonIgnore]
  public List<FormulaStructString> Strings { get; } = new();
  [JsonIgnore]
  public List<FormulaStructStringSubstitution> StringsToSubstitutionArgumentFrom { get; } = new();
  [JsonIgnore]
  public List<FormulaStructStringSubstitution> StringsToSubstitutionArgumentTo { get; } = new();
  [JsonIgnore]
  public List<InferenceFormulaStructArgumentMapping> InferenceFormulaStructArgumentMappings { get; } = new();
}