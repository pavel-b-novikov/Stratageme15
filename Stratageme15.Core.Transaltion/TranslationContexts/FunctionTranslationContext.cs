using System.Collections.Generic;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Transaltion.Builders;

namespace Stratageme15.Core.Transaltion.TranslationContexts
{
    public class FunctionTranslationContext
    {
        private readonly Stack<CodeBlock> _blocksStack = new Stack<CodeBlock>();

        public FunctionTranslationContext(SyntaxNode originalNode, FunctionDefExpression function)
        {
            OriginalDeclaringNode = originalNode;
            LocalVariables = new VariablesContext();
            Function = function;
            Function.IsScolonNeeded = true;
        }

        public FunctionTranslationContext(SyntaxNode originalNode,string functionName = null)
        {
            OriginalDeclaringNode = originalNode;
            LocalVariables = new VariablesContext();
            Function = new FunctionDefExpression();
            Function.IsScolonNeeded = true;
            if (!string.IsNullOrWhiteSpace(functionName))
            {
                Function.Name = functionName.Ident();
            }
        }

        public SyntaxNode OriginalDeclaringNode { get; private set; }
        public VariablesContext LocalVariables { get; private set; }
        public FunctionDefExpression Function { get; private set; }
        public StatementLabel NextStatementLabel { get; set; }
        public CodeBlock CurrentBlock
        {
            get { return _blocksStack.Peek(); }
        }

        public void PushBlock()
        {
            var blk = new CodeBlock();
            if (Function.Code==null)
            {
                Function.Code = blk;
                _blocksStack.Push(blk);
            }else
            {
                if (_blocksStack.Count==0)
                {
                    _blocksStack.Push(Function.Code);
                }
            }
            
            LocalVariables.PushContext();
        }

        public void PopBlock()
        {
            CodeBlock blk = _blocksStack.Pop();
            if (_blocksStack.Count > 0)
            {
                CurrentBlock.CollectSymbol(blk);
            }
            else
            {
                if (Function.Code==null)
                {
                    Function.Code = blk;    
                }
                
            }
            LocalVariables.PopContext();
        }
    }
}