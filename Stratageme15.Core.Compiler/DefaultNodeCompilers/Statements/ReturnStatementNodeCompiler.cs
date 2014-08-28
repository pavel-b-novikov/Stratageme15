using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class ReturnStatementNodeCompiler : StatementNodeCompilerBase<ReturnStatement>
    {
        protected override void CompileStatement(TextWriter output, ReturnStatement node)
        {
            output.Write("return");
            if (node.ReturnExpression!=null)
            {
                output.Write(" ");
                CompileChild(output,node.ReturnExpression);
            }
        }
    }
}