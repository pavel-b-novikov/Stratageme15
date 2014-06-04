using System.IO;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class CaseClauseNodeCompiler : NodeCompilerBase<CaseClause>
    {
        protected override void DoCompile(TextWriter output, CaseClause node)
        {
            output.Write("case ");
            CompileChild(output, node.CaseExpression);
            output.Write(": ");

            if (node.CodeBlock != null && node.CodeBlock.Statements.Count > 0)
            {
                IncTab(output);
                foreach (var ncb in node.CodeBlock.Statements)
                {
                    NewLine(output);
                    CompileChild(output, (SyntaxTreeNodeBase)ncb);
                    Scolon(output);
                    
                }
                DecTab(output);
            }


        }
    }
}