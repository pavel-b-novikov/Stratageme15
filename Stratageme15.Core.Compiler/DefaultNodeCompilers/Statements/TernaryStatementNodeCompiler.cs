using System.IO;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements
{
    public class TernaryStatementNodeCompiler : StatementNodeCompilerBase<TernaryStatement>
    {
        protected override void CompileStatement(TextWriter output, TernaryStatement node)
        {
            CompileChild(output,node.If);
            output.Write("?");
            if (node.Then!=null)
            {
                CompileChild(output,(SyntaxTreeNodeBase)node.Then);
            }

            if (node.ThenExpression != null)
            {
                CompileChild(output, (SyntaxTreeNodeBase)node.ThenExpression);
            }
            output.Write(":");

            if (node.Else != null)
            {
                CompileChild(output, (SyntaxTreeNodeBase)node.Else);
            }

            if (node.ElseExpression != null)
            {
                CompileChild(output, (SyntaxTreeNodeBase)node.ElseExpression);
            }

        }
    }
}