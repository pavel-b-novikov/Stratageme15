using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class DoWhileStatementNodeCompiler : StatementNodeCompilerBase<DoWhileStatement>
    {
        protected override void CompileStatement(TextWriter output, DoWhileStatement node)
        {
            output.Write("do ");
            CompileChild(output,node.CodeBlock);
            output.Write(" while ");
            CompileChild(output,node.WhileExpression);
        }
    }
}
