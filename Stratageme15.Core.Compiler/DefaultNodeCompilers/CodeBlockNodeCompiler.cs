using System.IO;
using Stratageme15.Core.Compiler.DefaultNodeCompilers.Statements;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Compiler.DefaultNodeCompilers
{
    public class CodeBlockNodeCompiler : StatementNodeCompilerBase<CodeBlock>
    {
        private void CompileStatement(TextWriter output, IStatement stm,bool newLine = true)
        {
            if (newLine) NewLine(output);
            CompileChild(output, (SyntaxTreeNodeBase)stm);
            if (!(
                    (stm is SwitchStatement)
                    || (stm is ForStatement)
                    || (stm is IfStatement)
                    || (stm is ForOfStatement)
                    || (stm is ForInStatement)
                    || (stm is TryCatchFinallyStatement)
                    || (stm is WhileStatement)
                ))
            Scolon(output);
        }
        protected override void CompileStatement(TextWriter output, CodeBlock node)
        {
            while (node.Statements.Count == 1 && node.Statements[0] is CodeBlock)
            {
                node = (CodeBlock)node.Statements[0];
            }
            if (node.Statements.Count == 0)
            {
                output.Write("{ }"); return;
            }

            var stm = node.Statements[0];
            if (node.Statements.Count == 1 &&
                    !(
                        (stm is SwitchStatement)
                        || (stm is ForStatement)
                        || (stm is IfStatement)
                        || (stm is ForOfStatement)
                        || (stm is ForInStatement)
                        || (stm is TryCatchFinallyStatement)
                        || (stm is WhileStatement)
                    )
                )
            {
                
                output.Write("{ ");
                CompileStatement(output, node.Statements[0],false);
                output.Write(" }");
                return;
            }
            output.Write("{");
            IncTab(output);
            foreach (var statement in node.Statements)
            {
                CompileStatement(output,statement);
            }
            DecTab(output);
            NewLine(output);
            output.Write("}");
        }
    }
}