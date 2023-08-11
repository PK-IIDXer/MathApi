using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MathApi.Models;

public class Theorem
{
  public long Id { get; set; }
  public string Name { get; set; } = "";
  public bool IsInference { get; set; } = false;
  public bool IsProved { get; set; } = false;

  private List<Symbol>? _FreeAndPropVariables = null;
  /// <summary>
  /// 定理に含まれる相異なる自由・命題変数のリスト
  /// ※TheoremAssumptions, TheoremConclusions、
  /// 　およびそれらのFormulaStringおよびFormulaString.Symbolのインクルードが必要
  /// </summary>
  [NotMapped]
  [JsonIgnore]
  public List<Symbol> FreeAndPropVariables
  {
    get
    {
      if (_FreeAndPropVariables != null)
        return _FreeAndPropVariables;

      _FreeAndPropVariables = new List<Symbol>();
      foreach (var ta in TheoremAssumptions)
      {
        foreach (var s in ta.Formula?.FreeAndPropVariables ?? throw new Exception("Include TheoremAssumption.Formula"))
        {
          if (_FreeAndPropVariables.Any(r => r.Id == s.Id))
            continue;
          _FreeAndPropVariables.Add(s);
        }
      }
      foreach (var ta in TheoremConclusions)
      {
        foreach (var s in ta.Formula?.FreeAndPropVariables ?? throw new Exception("Include TheoremAssumption.Formula"))
        {
          if (_FreeAndPropVariables.Any(r => r.Id == s.Id))
            continue;
          _FreeAndPropVariables.Add(s);
        }
      }
      return _FreeAndPropVariables;
    }
  }

  public List<TheoremAssumption> TheoremAssumptions { get; set; } = new();
  public List<TheoremConclusion> TheoremConclusions { get; set; } = new();

  public Inference? Inference { get; }

  public List<Proof>? Proofs { get; } = new();
}
