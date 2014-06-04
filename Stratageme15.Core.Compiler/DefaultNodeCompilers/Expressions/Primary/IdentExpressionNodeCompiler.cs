using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions.Primary
{
    public class IdentExpressionNodeCompiler : ExpressionNodeCompilerBase<IdentExpression>
    {
        protected override void CompileExpression(TextWriter output, IdentExpression node)
        {
            output.Write(node.Identifier);
        }
    }
}