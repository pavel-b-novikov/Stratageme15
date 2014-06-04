using System.IO;
using Stratageme15.Core.Compiler.DefaultNodeCompilers.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{

    public class DeleteStatementNodeCompiler : ExpressionNodeCompilerBase<DeleteStatement>
    {
        protected override void CompileExpression(TextWriter output, DeleteStatement node)
        {
            output.Write("delete ");
            CompileChild(output,node.ExpressionToDelete);
        }
    }
}
