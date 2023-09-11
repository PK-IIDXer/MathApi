using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

[PrimaryKey(nameof(FormulaStructId), nameof(SerialNo))]
public class FormulaStructArgument
{
  public FormulaStruct FormulaStruct { get; set; } = new();
  public long FormulaStructId { get; set; }
  public int SerialNo { get; set; }
  public FormulaLabel? Label { get; }
  public int LabelId { get; set; }

  public List<FormulaStructString> StringsToBoundArgument { get; } = new();
  public List<FormulaStructString> Strings { get; } = new();
  public List<FormulaStructStringSubstitution> StringsToSubstitutionArgumentFrom { get; } = new();
  public List<FormulaStructStringSubstitution> StringsToSubstitutionArgumentTo { get; } = new();
  public List<InferenceFormulaStructArgumentMapping> InferenceFormulaStructArgumentMappings { get; } = new();
}