using System;
using ArrayVisualizerExt.ArrayLoaders;

namespace ArrayVisualizerExt.TypeParsers
{
    public class DefaultParser : ITypeParser
    {
        private readonly IArrayLoader _arrayLoader;

        public DefaultParser(IArrayLoader arrayLoader)
        {
            this._arrayLoader = arrayLoader;
        }

        #region ITypeParser Members

        public char LeftBracket { get; set; }
        public char RightBracket { get; set; }
        public Func<string, int> ParseDimension { get; set; }

        public bool IsExpressionTypeSupported(EnvDTE.Expression expression)
        {
            return _arrayLoader.IsExpressionArrayType(expression);
        }

        public string GetDisplayName(EnvDTE.Expression expression)
        {
            return _arrayLoader.GetDisplayName(expression);
        }

        public int[] GetDimensions(EnvDTE.Expression expression)
        {
            return _arrayLoader.GetDimensions(expression);
        }

        public int GetMembersCount(EnvDTE.Expression expression)
        {
            return _arrayLoader.GetMembersCount(expression);
        }

        public object[] GetValues(EnvDTE.Expression expression)
        {
            return _arrayLoader.GetValues(expression);
        }

        #endregion
    }
}