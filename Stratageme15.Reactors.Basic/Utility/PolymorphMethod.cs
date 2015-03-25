using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using CSharpExtensions = Microsoft.CodeAnalysis.CSharp.CSharpExtensions;

namespace Stratageme15.Reactors.Basic.Utility
{
    public class PolymorphMethod
    {
        private const string _argumentsVal = "arguments";
        private readonly string _methodName;

        public string MethodName
        {
            get { return _methodName; }
        }

        private readonly IfStatement _polymorphIfStatement;
        private readonly TranslationContextWrapper _context;
        
        public IfStatement PolymorphIfStatement
        {
            get { return _polymorphIfStatement; }
        }

        public PolymorphMethod(string methodName, TranslationContextWrapper context)
        {
            _methodName = methodName;
            _context = context;
            _polymorphIfStatement = new IfStatement();
        }

        public CodeBlock AddOverload(ParameterListSyntax parameters)
        {
            var expr = BuildTypeCondition(parameters);
            _polymorphIfStatement.CollectSymbol(expr);
            CodeBlock cb = new CodeBlock();
            AppendLocalVariables(cb,parameters);
            _polymorphIfStatement.CollectSymbol(cb);
            return cb;
        }

        private void AppendLocalVariables(CodeBlock cb,ParameterListSyntax parameters)
        {
            int i = 0;
            foreach (var parameterSyntax in parameters.Parameters)
            {
                var vbl = parameterSyntax.Identifier.ValueText.Variable(_argumentsVal.ToIdent().Index(i));
                cb.CollectSymbol(vbl);
                i++;
            }
        }

        private ParenthesisExpression BuildTypeCondition(ParameterListSyntax parameters)
        {
            ParenthesisExpression current = null;
            int i = 0;
            foreach (var parameterSyntax in parameters.Parameters)
            {
                var left = _argumentsVal.ToIdent().Index(i).CallMember(JavascriptElements.GetFullQualifiedNameFunction);
                var symbolInfo = CSharpExtensions.GetSymbolInfo(_context.SemanticModel,parameterSyntax.Type);
                var type = (ITypeSymbol) symbolInfo.Symbol;
                var name = type.FullQualifiedName().Literal();
                var parentEx = left.Comparison(ComparisonOperator.Equal, name).Parenthesize();
                if (current != null)
                {
                    current = current.Logical(LogicalOperator.Or, parentEx).Parenthesize();
                }
                else
                {
                    current = parentEx;
                }
            }
            if (current==null) return new ParenthesisExpression();
            return current;
        }
    }
}
