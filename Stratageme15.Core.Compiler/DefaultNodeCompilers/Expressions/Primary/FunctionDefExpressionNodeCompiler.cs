using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions.Primary
{
    public class FunctionDefExpressionNodeCompiler : ExpressionNodeCompilerBase<FunctionDefExpression>
    {

        protected override void CompileExpression(TextWriter output, FunctionDefExpression node)
        {
            
            output.Write("function ");
            if (node.Name != null) CompileChild(output,node.Name);
            if (node.Parameters != null) CompileChild(output,node.Parameters);
            else output.Write("()");
            output.Write(" ");
            IncTab(output);
            if (node.Code != null) CompileChild(output,node.Code);
            DecTab(output);
        }
    }
}