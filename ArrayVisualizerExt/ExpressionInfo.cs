using EnvDTE;

namespace ArrayVisualizerExt
{
    public class ExpressionInfo
    {
        public ExpressionInfo(string name, string sectionType, string value, Expression expression, int sectionCode)
        {
            Name = name;
            Section = sectionType;
            SectionCode = sectionCode;
            Expression = expression;
            Value = value;
        }

        public string FullName
        {
            get { return Section + Name + " - " + Value; }
        }

        public int SectionCode { get; set; }
        public string Name { get; set; }
        public string Section { get; set; }
        public string Value { get; set; }
        public Expression Expression { get; set; }
    }
}