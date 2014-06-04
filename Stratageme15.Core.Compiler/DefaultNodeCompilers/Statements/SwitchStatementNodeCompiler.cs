using System.IO;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class SwitchStatementNodeCompiler : StatementNodeCompilerBase<SwitchStatement>
    {
        protected override void CompileStatement(TextWriter output, SwitchStatement node)
        {
            output.Write("switch ");
            CompileChild(output,node.SwitchExpression);
            output.WriteLine("{");
            IncTab(output);
            foreach (var caseClause in node.Cases)
            {
                CompileChild(output,caseClause);
                NewLine(output);
            }
            CompileChild(output,node.Default);
            NewLine(output);
            DecTab(output);
            output.WriteLine("}");
        }
    }
}