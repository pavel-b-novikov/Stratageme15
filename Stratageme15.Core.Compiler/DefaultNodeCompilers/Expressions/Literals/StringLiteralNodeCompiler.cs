using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions.Literals
{
    class StringLiteralNodeCompiler : ExpressionNodeCompilerBase<StringLiteral>
    {
        protected override void CompileExpression(TextWriter output, StringLiteral node)
        {
            if (node.IsQuoted)
            {
                output.Write(node.Literal);
                return;
            }
            var s = node.Literal;
            if (s.Contains("\"")) s = s.Replace("\"", "\\\"");
            if (s.Contains("\r")) s = s.Replace("\r", "\\r");
            if (s.Contains("\n")) s = s.Replace("\n", "\\n");
            if (s.Contains("\t")) s = s.Replace("\n", "\\t");
            s = string.Format("\"{0}\"", s);
            output.Write(s);
        }
    }
}
