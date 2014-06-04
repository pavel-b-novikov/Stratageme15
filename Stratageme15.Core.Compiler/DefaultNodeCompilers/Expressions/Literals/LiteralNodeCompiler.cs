using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions.Literals
{
    class LiteralNodeCompiler : ExpressionNodeCompilerBase<LiteralExpression>
    {
        protected override void CompileExpression(TextWriter output, LiteralExpression node)
        {
            output.Write(node.Literal);
        }
    }
}
