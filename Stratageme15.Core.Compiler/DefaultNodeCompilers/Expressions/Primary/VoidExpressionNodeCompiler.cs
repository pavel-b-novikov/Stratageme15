using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions.Primary
{
    public class VoidExpressionNodeCompiler : ExpressionNodeCompilerBase<VoidExpression>
    {
        protected override void CompileExpression(TextWriter output, VoidExpression node)
        {
            output.Write("void (");
            CompileChild(output,node.ExpressionToBeVoid);
            output.Write(")");
        }
    }
}