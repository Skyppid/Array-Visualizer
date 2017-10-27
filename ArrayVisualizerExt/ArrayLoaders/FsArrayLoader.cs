using System;
using System.Collections.Generic;
using System.Linq;
using ArrayVisualizerExt.TypeParsers;
using EnvDTE;

namespace ArrayVisualizerExt.ArrayLoaders
{
  internal class FsArrayLoader : IArrayLoader
  {
    #region IArrayLoader Members

    public char LeftBracket { get { return '['; } }

    public char RightBracket { get { return ']'; } }

    public bool IsExpressionArrayType(Expression expression)
    {
      if (expression == null)
        throw new ArgumentNullException("expression"); 
      
      string expressionType = Helper.RemoveBrackets(expression.Type);
      return expressionType.EndsWith("]") && (expressionType.EndsWith("[]") || expressionType.EndsWith("[,]") || expressionType.EndsWith("[,,]") || expressionType.EndsWith("[,,,]"));
    }

    public string GetDisplayName(Expression expression)
    {
      if (expression == null)
        throw new ArgumentNullException("expression"); 
      
      return expression.Value;
    }

    public IEnumerable<ExpressionInfo> GetArrays(string section, Expression expression, ParsersCollection parsers, int sectionCode)
    {
      if (expression.DataMembers.Count == 0)
        yield break;

      foreach (ITypeParser parser in parsers.Where(P => P.IsExpressionTypeSupported(expression)))
      {
        yield return new ExpressionInfo(expression.Name, section, parser.GetDisplayName(expression), expression, sectionCode);
        break;
      }
    }

    public int GetMembersCount(Expression expression)
    {
      if (expression == null)
        throw new ArgumentNullException("expression"); 
      
      return expression.DataMembers.Count;
    }

    public int[] GetDimensions(Expression expression)
    {
      if (expression == null)
        throw new ArgumentNullException("expression"); 
      
      string dims = expression.Value;
      dims = dims.Substring(dims.IndexOf(LeftBracket) + 1);
      dims = dims.Substring(0, dims.IndexOf(RightBracket));

      int[] dimenstions = dims.Split(',').Select(X => ParseDimension(X.Trim())).ToArray();

      return dimenstions;
    }

    public int ParseDimension(string value)
    {
      if (value == null)
        throw new ArgumentNullException("value"); 
      
      if (value.StartsWith("0x"))
        return int.Parse(value.Substring(2), System.Globalization.NumberStyles.HexNumber);
      else
        return int.Parse(value);
    }

    public object[] GetValues(Expression expression)
    {
      if (expression == null)
        throw new ArgumentNullException("expression"); 
      
      object[] values;     
      
      if (expression.DataMembers.Item(1).Type.IndexOf(LeftBracket) != -1)
        values = expression.DataMembers.Cast<Expression>().ToArray();
      else
        values = expression.DataMembers.Cast<Expression>().Select(E => E.Value).ToArray();
      return values;
    }

    #endregion
  }
}
