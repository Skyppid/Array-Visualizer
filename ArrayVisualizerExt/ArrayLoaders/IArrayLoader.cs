using System.Collections.Generic;
using ArrayVisualizerExt.TypeParsers;
using EnvDTE;

namespace ArrayVisualizerExt.ArrayLoaders
{
    public interface IArrayLoader
    {
        char LeftBracket { get; }

        char RightBracket { get; }

        int ParseDimension(string dimension);

        bool IsExpressionArrayType(Expression expression);

        string GetDisplayName(Expression expression);

        IEnumerable<ExpressionInfo> GetArrays(string section, Expression expression, ParsersCollection parsers,
            int sectionCode);

        int[] GetDimensions(Expression expression);

        int GetMembersCount(Expression expression);

        object[] GetValues(Expression expression);
    }
}