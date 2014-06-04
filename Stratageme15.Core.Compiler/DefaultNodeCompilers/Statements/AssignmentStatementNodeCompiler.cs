using System.IO;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{

    public class AssignmentStatementNodeCompiler : StatementNodeCompilerBase<AssignmentStatement>
    {
        protected override void CompileStatement(TextWriter output, AssignmentStatement node)
        {
            CompileChild(output,node.Identifier);
            if (node.Indexer!=null) CompileChild(output,node.Indexer);
            output.Write(" ");
            output.Write(OperatorConverter.OperatorString(node.Operator));
            output.Write(" ");
            CompileChild(output,node.AssignmentExpression);
        }
    }
}