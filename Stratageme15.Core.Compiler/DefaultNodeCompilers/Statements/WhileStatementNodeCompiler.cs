using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class WhileStatementNodeCompiler : StatementNodeCompilerBase<WhileStatement>
    {
        protected override void CompileStatement(TextWriter output, WhileStatement node)
        {
            output.Write("while ");
            CompileChild(output,node.WhileCondition);
            output.Write(" ");
            CompileChild(output,node.WhileBlock);
        }
    }
}