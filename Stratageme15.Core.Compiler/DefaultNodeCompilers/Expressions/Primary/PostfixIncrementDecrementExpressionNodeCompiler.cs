using System.IO;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions.Primary
{
    public class PostfixIncrementDecrementExpressionNodeCompiler : ExpressionNodeCompilerBase<PostfixIncrementDecrementExpression>
    {
        protected override void CompileExpression(TextWriter output, PostfixIncrementDecrementExpression node)
        {
            CompileChild(output,node.Callee);
            output.Write(OperatorConverter.OperatorString(node.Operator));
        }
    }
}
