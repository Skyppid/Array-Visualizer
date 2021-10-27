using System;
using System.Collections.Generic;
using ArrayVisualizerExt.ArrayLoaders;

namespace ArrayVisualizerExt.TypeParsers
{
    public class ParsersCollection : IEnumerable<ITypeParser>
    {
        private readonly List<ITypeParser> _parsers;
        private readonly IArrayLoader _arrayLoader;

        public ParsersCollection(IArrayLoader arrayLoader, IEnumerable<Type> selectedParsers)
        {
            if (selectedParsers == null)
                throw new ArgumentNullException(nameof(selectedParsers));

            _arrayLoader = arrayLoader ?? throw new ArgumentNullException(nameof(arrayLoader));
            _parsers = new List<ITypeParser>();

            foreach (Type parserType in selectedParsers)
                AddParser((ITypeParser) Activator.CreateInstance(parserType));

            AddParser(new DefaultParser(arrayLoader)); //must be last!
        }

        private void AddParser(ITypeParser parser)
        {
            parser.LeftBracket = _arrayLoader.LeftBracket;
            parser.RightBracket = _arrayLoader.RightBracket;
            parser.ParseDimension = _arrayLoader.ParseDimension;
            _parsers.Add(parser);
        }

        #region IEnumerable<ITypeParser> Members

        public IEnumerator<ITypeParser> GetEnumerator()
        {
            return _parsers.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}