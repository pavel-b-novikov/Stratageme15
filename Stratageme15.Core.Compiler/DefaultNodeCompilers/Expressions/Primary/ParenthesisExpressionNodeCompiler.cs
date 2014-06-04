using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions.Primary
{
    public class ParenthesisExpressionNodeCompiler : ExpressionNodeCompilerBase<ParenthesisExpression>
    {
        protected override void CompileExpression(TextWriter output, ParenthesisExpression node)
        {
            output.Write("(");
            CompileChild(output,node.InnerExpression);
            output.Write(")");
        }
    }
}
