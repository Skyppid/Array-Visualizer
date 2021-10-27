using System;
using System.Collections.Generic;
using System.Linq;
using ArrayVisualizerExt.TypeParsers;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace ArrayVisualizerExt.ArrayLoaders
{
    internal class VbArrayLoader : IArrayLoader
    {
        #region IArrayLoader Members

        public char LeftBracket => '(';

        public char RightBracket => ')';

        public bool IsExpressionArrayType(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            ThreadHelper.ThrowIfNotOnUIThread();

            var expressionType = expression.Type == "System.Array" ? expression.Value : expression.Type;

            expressionType = Helper.RemoveBrackets(expressionType);
            return expressionType.EndsWith(")") && (expressionType.EndsWith("()") || expressionType.EndsWith("(,)") ||
                                                    expressionType.EndsWith("(,,)") ||
                                                    expressionType.EndsWith("(,,,)"));
        }

        public string GetDisplayName(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            ThreadHelper.ThrowIfNotOnUIThread();

            if (expression.Type == "System.Array")
                return GetDisplayName(expression.DataMembers.Item(1));

            string expType = expression.Type;
            expType = expType.Substring(0, expType.IndexOf(LeftBracket));
            expType = expType + LeftBracket + string.Join(",", GetDimensions(expression)) + RightBracket;
            return expType;
        }

        public IEnumerable<ExpressionInfo> GetArrays(string section, Expression expression, ParsersCollection parsers,
            int sectionCode)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (expression.Value == "Nothing" || expression.DataMembers.Count == 0)
                yield break;

            foreach (ITypeParser parser in parsers.Where(p => p.IsExpressionTypeSupported(expression)))
            {
                yield return new ExpressionInfo(expression.Name, section, parser.GetDisplayName(expression), expression,
                    sectionCode);
                break;
            }

            if (expression.Name == "Me")
                foreach (Expression subExpression in expression.DataMembers)
                foreach (ExpressionInfo item in GetArrays("Me.", subExpression, parsers, -1))
                    yield return item;
        }

        public int GetMembersCount(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            ThreadHelper.ThrowIfNotOnUIThread();

            if (expression.Type == "System.Array")
                return GetMembersCount(expression.DataMembers.Item(1));

            return expression.DataMembers.Count;
        }

        public int[] GetDimensions(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            ThreadHelper.ThrowIfNotOnUIThread();

            if (expression.Type == "System.Array")
                return GetDimensions(expression.DataMembers.Item(1));

            int last = expression.DataMembers.Count;
            string dims = expression.DataMembers.Item(last).Name;
            dims = dims.Substring(dims.IndexOf(LeftBracket) + 1);
            dims = dims.Substring(0, dims.IndexOf(RightBracket));

            int[] dimenstions = dims.Split(',').Select(x => ParseDimension(x.Trim()) + 1).ToArray();
            return dimenstions;
        }

        public int ParseDimension(string dimensionString)
        {
            if (string.IsNullOrEmpty(dimensionString))
                throw new ArgumentNullException(nameof(dimensionString));

            return dimensionString.StartsWith("&H") ? int.Parse(dimensionString.Substring(2), System.Globalization.NumberStyles.HexNumber) : int.Parse(dimensionString);
        }

        public object[] GetValues(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            ThreadHelper.ThrowIfNotOnUIThread();

            if (expression.Type == "System.Array")
                return GetValues(expression.DataMembers.Item(1));

            object[] values;
            if (expression.DataMembers.Item(1).Type.IndexOf(LeftBracket) != -1)
                values = expression.DataMembers.Cast<Expression>().ToArray();
            else
                values = expression.DataMembers.Cast<Expression>().Select(e => e.Value).ToArray();
            return values;
        }

        #endregion
    }
}