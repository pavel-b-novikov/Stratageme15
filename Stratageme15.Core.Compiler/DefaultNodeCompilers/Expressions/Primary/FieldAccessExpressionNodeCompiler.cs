using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions.Primary
{
    public class FieldAccessExpressionNodeCompiler : ExpressionNodeCompilerBase<FieldAccessExpression>
    {
        protected override void CompileExpression(TextWriter output, FieldAccessExpression node)
        {
            CompileChild(output,node.Accessee);
            if (node.Member!=null)
            {
                output.Write(".");
                CompileChild(output, node.Member);
            }
        }
    }
}