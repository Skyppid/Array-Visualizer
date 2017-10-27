using System;
using System.Collections.Generic;
using System.Linq;
using ArrayVisualizerExt.TypeParsers;
using EnvDTE;

namespace ArrayVisualizerExt.ArrayLoaders
{
  internal class CsArrayLoader : IArrayLoader
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

      switch (expression.Name)
      {
        case "this":
          foreach (Expression subExpression in expression.DataMembers)
            foreach (ExpressionInfo item in GetArrays("this.", subExpression, parsers, -1))
              yield return item;
          break;
        case "Static members":
          foreach (Expression subExpression in expression.DataMembers)
            foreach (ExpressionInfo item in GetArrays("(Static) ", subExpression, parsers, -2))
              yield return item;
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

   public int ParseDimension(string dimensionString)
    {
      if (dimensionString == null)
        throw new ArgumentNullException("dimensionString"); 
     
     if (dimensionString.StartsWith("0x"))
        return int.Parse(dimensionString.Substring(2), System.Globalization.NumberStyles.HexNumber);
      else
        return int.Parse(dimensionString);
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
