namespace MathApi.Const;

/// <summary>
/// 文字タイプ
/// </summary>
public enum SymbolType : long {
  FreeVariable = 1,
  BoundVariable = 2,
  PropositionVariable = 3,
  Constant = 4,
  Function = 5,
  Predicate = 6,
  Logic = 7,
  TermQuantifier = 8,
  PropositionQuantifier = 9
}

/// <summary>
/// 論理式タイプ
/// </summary>
public enum FormulaType : long {
  Term = 1,
  Proposition = 2
}

/// <summary>
/// 推論規則引数タイプ
/// </summary>
public enum InferenceArgumentType : int {
  Term = 1,
  Proposition = 2,
  FreeVariable = 3
}

/// <summary>
/// 推論規則仮定解消タイプ
/// </summary>
public enum InferenceAssumptionDissolutionType : int {
  None = 1,
  Required = 2,
  Necessary = 3
}