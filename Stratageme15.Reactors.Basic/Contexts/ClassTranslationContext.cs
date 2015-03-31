using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Binary;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Contexts
{
    /// <summary>
    /// Holds information of class being translated
    /// </summary>
    public class ClassTranslationContext
    {
        /// <summary>
        /// Reference to current translation context
        /// </summary>
        private readonly TranslationContextWrapper _context;

        private readonly Stack<FunctionTranslationContext> _functionsStack = new Stack<FunctionTranslationContext>();

        private bool _outOfContext;

        public ClassTranslationContext(ClassDeclarationSyntax originalNode, ITypeSymbol type, TranslationContextWrapper context)
        {
            OriginalNode = originalNode;
            Type = type;
            _context = context;
            ClassScope = JavascriptHelper.CreateEmptyFunction();
            Constructor = JavascriptHelper.CreateEmptyFunction(originalNode.Identifier.ValueText);
            FieldsDefinitionBlock = new CodeBlock();
            _context.Context.PushContextNode(ClassScope.Code);
        }
        private void AppendTypeHelpers(CodeBlock block,ClassDeclarationSyntax originalNode)
        {
            var ns = originalNode.FullNamespace();
            var fq = originalNode.FullQualifiedName();

            var fullQualified = originalNode.Identifier.ValueText.ToIdent()
                .Prototype(JavascriptElements.GetFullQualifiedNameFunction)
                .Assignment(JavascriptHelper.CreateEmptyFunction().AppendStatement(fq.Literal().ReturnIt()));
            var nsFunc = originalNode.Identifier.ValueText.ToIdent()
                .Prototype(JavascriptElements.GetNamespaceFunction)
                .Assignment(JavascriptHelper.CreateEmptyFunction().AppendStatement(ns.Literal().ReturnIt()));
            var fullQualifiedType = originalNode.Identifier.ValueText.ToIdent()
                .Member(JavascriptElements.GetFullQualifiedNameFunction)
                .Assignment(JavascriptHelper.CreateEmptyFunction().AppendStatement(fq.Literal().ReturnIt()));
            var nsFuncType = originalNode.Identifier.ValueText.ToIdent()
                .Member(JavascriptElements.GetNamespaceFunction)
                .Assignment(JavascriptHelper.CreateEmptyFunction().AppendStatement(ns.Literal().ReturnIt()));
            block.CollectSymbol(fullQualified);
            block.CollectSymbol(nsFunc);
            block.CollectSymbol(fullQualifiedType);
            block.CollectSymbol(nsFuncType);
        }

        public SyntaxTreeNodeBase EmitClassDeclaration()
        {
            var ns = OriginalNode.FullNamespace();
            SyntaxTreeNodeBase resultNode;
            //todo inheritance here
            if (FieldsDefinitionBlock.Statements.Count > 0)
            {
                Constructor.Code.CollectSymbolAtStart(FieldsDefinitionBlock);
            }
            ClassScope.Code.CollectSymbolAtStart(Constructor);
            foreach (var assignmentBinaryExpression in _nestedClassesAssignments)
            {
                ClassScope.Code.CollectSymbol(assignmentBinaryExpression);
            }
            AppendTypeHelpers(ClassScope.Code,OriginalNode);
            ClassScope.Code.CollectSymbol(OriginalNode.Identifier.ValueText.ReturnIt());
            if (string.IsNullOrEmpty(ns))
            {
                resultNode = OriginalNode.Identifier.ValueText.Variable(ClassScope.Parenthesize().Call());
            }else
            {
                resultNode = JavascriptElements.NamespaceFunction.ToIdent().Call(
                        ns.Literal(), 
                        ClassScope.Parenthesize().Call());
            }
            _context.Context.PopContextNode();
            return resultNode;
        }

        /// <summary>
        /// Original C# node with class description
        /// </summary>
        public ClassDeclarationSyntax OriginalNode { get; private set; }

        /// <summary>
        /// Translating class type
        /// </summary>
        public ITypeSymbol Type { get; set; }

        /// <summary>
        /// Class scope function
        /// </summary>
        public FunctionDefExpression ClassScope { get; private set; }

        /// <summary>
        /// Class constructor function
        /// </summary>
        public FunctionDefExpression Constructor { get; private set; }

        /// <summary>
        /// Code block for fields definitions and initialization
        /// </summary>
        public CodeBlock FieldsDefinitionBlock { get; private set; }

        /// <summary>
        /// Gets current function translation context
        /// </summary>
        public FunctionTranslationContext CurrentFunction
        {
            get { return _outOfContext ? null : _functionsStack.Peek(); }
        }


        /// <summary>
        /// Temporary disables access to function context due to going out of method translation
        /// </summary>
        public void OutOfContext()
        {
            _outOfContext = true;
        }

        /// <summary>
        /// Enables access to function context after OutOfContext call
        /// </summary>
        public void ReturnToContext()
        {
            _outOfContext = false;
        }

        #region Nesting

        private List<AssignmentBinaryExpression> _nestedClassesAssignments = new List<AssignmentBinaryExpression>();

        /// <summary>
        /// Parent class (only for nested classes)
        /// </summary>
        public ClassTranslationContext Parent { get; private set; }

        /// <summary>
        /// Is current class context nested
        /// </summary>
        public bool IsNested { get; private set; }

        public ClassTranslationContext Nest(ClassDeclarationSyntax originalNode, ITypeSymbol type)
        {
            ClassTranslationContext nested = new ClassTranslationContext(originalNode,type,_context);
            nested.IsNested = true;
            nested.Parent = this;
            return nested;
        }

        public ClassTranslationContext NestEnd()
        {
            string name = OriginalNode.Identifier.ValueText;
            if (FieldsDefinitionBlock.Statements.Count > 0)
            {
                Constructor.Code.CollectSymbolAtStart(FieldsDefinitionBlock);
            }
            ClassScope.Code.CollectSymbol(Constructor);
            AssignmentBinaryExpression assgn = Parent.OriginalNode.Identifier.ValueText.ToIdent().Member(name).Assignment(name);
            foreach (var statement in ClassScope.Code.Statements)
            {
                Parent.ClassScope.Code.CollectSymbol((SyntaxTreeNodeBase) statement);
            }
            AppendTypeHelpers(Parent.ClassScope.Code, OriginalNode);
            Parent._nestedClassesAssignments.Add(assgn);
            _context.Context.PopContextNode();
            return Parent;
        }
        #endregion

        public void PushFunction(SyntaxNode originalNode, string name = null)
        {
            _functionsStack.Push(new FunctionTranslationContext(_context.Context, originalNode, name));
        }

        public void PushFunction(SyntaxNode originalNode, FunctionDefExpression existing)
        {
            _functionsStack.Push(new FunctionTranslationContext(_context.Context, originalNode, existing));
        }

        public void PopFunction()
        {
            _functionsStack.Pop();
        }

        

        #region Custom class translation context variables
        /// <summary>
        /// Custom class translation context variables
        /// </summary>
        private readonly Dictionary<string, object> _customContextVars = new Dictionary<string, object>();

        public object this[string s]
        {
            get { return _customContextVars[s]; }
            set { _customContextVars[s] = value; }
        }

        public T GetCustomVariable<T>(string s)
        {
            return (T)_customContextVars[s];
        }

        public bool IsCustomVariableDefined(string s)
        {
            return _customContextVars.ContainsKey(s);
        }

        public void DropCustomVariable(string s)
        {
            _customContextVars.Remove(s);
        }
        #endregion
    }
}