﻿using System.IO;
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
            if (NeedColon(stm)) Scolon(output);
        }
        private bool NeedColon(IStatement stm)
        {
            return !(
                        (stm is SwitchStatement)
                        || (stm is ForStatement)
                        || (stm is IfStatement)
                        || (stm is ForOfStatement)
                        || (stm is ForInStatement)
                        || (stm is TryCatchFinallyStatement)
                        || (stm is WhileStatement)
                        || (stm is CodeBlock)
                    );
        }
        protected override void CompileStatement(TextWriter output, CodeBlock node)
        {
            while (node.Statements.Count == 1 && node.Statements.First.Value is CodeBlock)
            {
                node = (CodeBlock)node.Statements.First.Value;
            }
            if (node.Statements.Count == 0)
            {
                if (node.IsEnclosed) output.Write("{ }"); 
                return;
            }

            var stm = node.Statements.First.Value;
            if (node.Statements.Count == 1 && NeedColon(stm))
            {
                
                if (node.IsEnclosed) output.Write("{ ");
                CompileStatement(output, node.Statements.First.Value,false);
                if (node.IsEnclosed) output.Write(" }");
                return;
            }
            if (node.IsEnclosed)  output.Write("{");
            IncTab(output);
            foreach (var statement in node.Statements)
            {
                CompileStatement(output,statement);
            }
            DecTab(output);
            NewLine(output);
            if (node.IsEnclosed)  output.Write("}");
        }
    }
}