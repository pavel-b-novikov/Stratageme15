using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom.Extensions
{
    public static class VariousExtensions
    {
        public static CodeBlock WrapInCodeBlock(this IStatement s)
        {
            CodeBlock c = new CodeBlock();
            c.Parent = ((SyntaxTreeNodeBase)s).Parent;
            c.Statements.Add(s);
            return c;
        }

        public static CodeBlock WrapInCodeBlock(this SyntaxTreeNodeBase s)
        {
            CodeBlock c = new CodeBlock();
            c.Parent = s.Parent;
            c.Statements.Add((IStatement) s);
            s.Parent = c;
            return c;
        }
    }
}
