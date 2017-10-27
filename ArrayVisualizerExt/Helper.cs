namespace ArrayVisualizerExt
{
  internal static class Helper
  {
    internal static string RemoveBrackets(string expressionType)
    {
      return expressionType.Replace("}", "").Replace("{", "");
    }
  }
}