using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions.Primary
{
    public class CallExpressionNodeCompiler : ExpressionNodeCompilerBase<CallExpression>
    {
        protected override void CompileExpression(TextWriter output, CallExpression node)
        {
            CompileChild(output,node.Callee);
            if (node.Parameters == null) output.Write("()");
            else
            {
                CompileChild(output, node.Parameters);
            }
        }
    }
}
