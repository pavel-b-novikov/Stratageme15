using System.IO;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions.Primary
{
    class PrefixIncrementDecrementExpressionNodeCompiler : ExpressionNodeCompilerBase<PrefixIncrementDecrementExpression>
    {
        protected override void CompileExpression(TextWriter output, PrefixIncrementDecrementExpression node)
        {
            output.Write(OperatorConverter.OperatorString(node.Operator));
            CompileChild(output, node.Callee);
        }
    }
}
