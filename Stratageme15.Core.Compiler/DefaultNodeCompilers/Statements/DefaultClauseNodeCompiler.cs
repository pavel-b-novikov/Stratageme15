using System.IO;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class DefaultClauseNodeCompiler : NodeCompilerBase<DefaultClause>
    {
        protected override void DoCompile(TextWriter output, DefaultClause node)
        {
            output.Write("default: ");
            IncTab(output);
            foreach (var op in node.DefaultBlock.Statements)
            {
                NewLine(output);
                CompileChild(output,(SyntaxTreeNodeBase)op);
                Scolon(output);
            }
            DecTab(output);
        }
    }
}