using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions.Primary
{
    public class NewInvokationExpressionNodeCompiler : ExpressionNodeCompilerBase<NewInvokationExpression>
    {
        protected override void CompileExpression(TextWriter output, NewInvokationExpression node)
        {
            output.Write("new ");
            var fa = CompileChildToString(node.IdentifierChain);

            if (!string.IsNullOrWhiteSpace(fa)) fa = fa.Trim('.');
            
            output.Write(fa);

            if (node.Parameters != null) CompileChild(output, node.Parameters);
        }
    }
}