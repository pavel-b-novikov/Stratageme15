using System.IO;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions
{

    public class UnaryExpressionNodeCompiler : ExpressionNodeCompilerBase<UnaryExpression>
    {
        protected override void CompileExpression(TextWriter output, UnaryExpression node)
        {
            if (node.FirstOperator!=null)
            {
                output.Write(OperatorConverter.OperatorString(node.FirstOperator.Value));
            }
            CompileChild(output,node.Operand);
        }
    }
}