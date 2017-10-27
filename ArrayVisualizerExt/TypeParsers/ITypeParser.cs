using System;
using EnvDTE;

namespace ArrayVisualizerExt.TypeParsers
{
    public interface ITypeParser
    {
        bool IsExpressionTypeSupported(Expression expression);

        string GetDisplayName(Expression expression);
        int[] GetDimensions(Expression expression);
        int GetMembersCount(Expression expression);
        object[] GetValues(Expression expression);

        Func<string, int> ParseDimension { get; set; }

        char LeftBracket { get; set; }
        char RightBracket { get; set; }
    }
}