using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals.KeywordLiterals;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Transaltion.Builders;

namespace Stratageme15.Core.Transaltion.TranslationContexts
{
    public class FunctionTranslationContext
    {
        private const string ClosureVariableName = "___self";

        private readonly Stack<CodeBlock> _blocksStack = new Stack<CodeBlock>();
        private TranslationContext _context;

        public FunctionTranslationContext(TranslationContext context, SyntaxNode originalNode,
                                          FunctionDefExpression function)
        {
            OriginalDeclaringNode = originalNode;
            LocalVariables = new VariablesContext(context);
            Function = function;
            _context = context;
            Function.IsScolonNeeded = true;
        }

        public FunctionTranslationContext(TranslationContext context, SyntaxNode originalNode,
                                          string functionName = null)
        {
            OriginalDeclaringNode = originalNode;
            _context = context;
            LocalVariables = new VariablesContext(context);
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
            if (Function.Code == null)
            {
                Function.Code = blk;
                _blocksStack.Push(blk);
            }
            else
            {
                if (_blocksStack.Count == 0)
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
                if (Function.Code == null)
                {
                    Function.Code = blk;
                }
            }
            LocalVariables.PopContext();
        }

        private bool _isClosureVariableDefined = false;
        public void DefineClosureVariable()
        {
            if (_isClosureVariableDefined) return;
            VariableDefStatement vds = new VariableDefStatement();
            vds.Variables.Add(new Tuple<IdentExpression, AssignmentOperator, Expression>(ClosureVariableName.Ident(),AssignmentOperator.Set, new ThisKeywordLiteralExpression()));
            LocalVariables.DefineVariable(ClosureVariableName,_context.CurrentClassContext.Type);
            Function.Code.CollectSymbolAtStart(vds);
            _isClosureVariableDefined = true;
        }

        public ExpressionSyntax GetClosureIdentifier()
        {
            if (!_isClosureVariableDefined)
            {
                return SyntaxFactory.ThisExpression();
            }

            return SyntaxFactory.IdentifierName(ClosureVariableName);
        }

        public Expression GetJsClosureIdentifier()
        {
            if (_isClosureVariableDefined)
            {
                return new IdentExpression(ClosureVariableName);
            }
            return new ThisKeywordLiteralExpression();
        }
    }
}