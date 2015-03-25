using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals.KeywordLiterals;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Contexts
{
    /// <summary>
    /// Function translation context
    /// Enclosing local variables contextes
    /// </summary>
    public class FunctionTranslationContext
    {
        /// <summary>
        /// This-closure variables will be named as 
        /// </summary>
        private const string ClosureVariableName = "___self";

        /// <summary>
        /// Stack of nested syntax blocks
        /// </summary>
        private readonly Stack<CodeBlock> _blocksStack = new Stack<CodeBlock>();

        /// <summary>
        /// Reference to translation context
        /// </summary>
        private readonly TranslationContext _context;

        /// <summary>
        /// Constructs new function translation context
        /// </summary>
        /// <param name="context">Current translation context</param>
        /// <param name="originalNode">Original node where function is declared</param>
        /// <param name="function">Existing FunctionDefExpression (if any)</param>
        public FunctionTranslationContext(TranslationContext context, SyntaxNode originalNode,
                                          FunctionDefExpression function)
        {
            OriginalDeclaringNode = originalNode;
            LocalVariables = new VariablesContext(context);
            Function = function;
            _context = context;
            Function.IsScolonNeeded = true;
        }

        /// <summary>
        /// Constructs new function translation context
        /// </summary>
        /// <param name="context">Current translation context</param>
        /// <param name="originalNode">Original node where function was declared</param>
        /// <param name="functionName">Optionally function name</param>
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
                Function.Name = functionName.ToIdent();
            }
        }

        /// <summary>
        /// Corresponding source syntax node current function was declared in
        /// </summary>
        public SyntaxNode OriginalDeclaringNode { get; private set; }

        /// <summary>
        /// Function local variables context
        /// </summary>
        public VariablesContext LocalVariables { get; private set; }

        /// <summary>
        /// Resulting function
        /// </summary>
        public FunctionDefExpression Function { get; private set; }
       
        /// <summary>
        /// Current syntax block - the top of inner blocks stack
        /// </summary>
        public CodeBlock CurrentBlock
        {
            get { return _blocksStack.Peek(); }
        }

        /// <summary>
        /// Creates new syntax block inside function and defines new stack of local variables
        /// </summary>
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

        /// <summary>
        /// Closes syntax block defined with PushBlock and also pops context of local variabless
        /// </summary>
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

        /// <summary>
        /// Is closure variable specified in current function or not
        /// </summary>
        private bool _isClosureVariableDefined = false;

        /// <summary>
        /// Defines closure variable in current function (var _self = this;)
        /// If it is already defined then does nothing
        /// </summary>
        public void DefineClosureVariable(TypeInfo typeOfCurrentClass)
        {
            if (_isClosureVariableDefined) return;
            VariableDefStatement vds = new VariableDefStatement();
            vds.Variables.Add(new Tuple<IdentExpression, AssignmentOperator, Expression>(ClosureVariableName.ToIdent(),AssignmentOperator.Set, new ThisKeywordLiteralExpression()));
            LocalVariables.DefineVariable(ClosureVariableName,typeOfCurrentClass);
            Function.Code.CollectSymbolAtStart(vds);
            _isClosureVariableDefined = true;
        }

        /// <summary>
        /// Return current closure "this" identifier
        /// If translation goes inside closure block ther returns identifier of closure variable name (__self)
        /// </summary>
        /// <returns>Roslyn expression</returns>
        public ExpressionSyntax GetClosureIdentifier()
        {
            if (!_isClosureVariableDefined)
            {
                return SyntaxFactory.ThisExpression();
            }

            return SyntaxFactory.IdentifierName(ClosureVariableName);
        }

        /// <summary>
        /// Return current closure "this" identifier
        /// If translation goes inside closure block ther returns identifier of closure variable name (__self)
        /// </summary>
        /// <returns>Javascript CodeDom Expression</returns>
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