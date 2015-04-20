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
        private const string ArgumentsVal = "arguments";
        private readonly string _methodName;

        public string MethodName
        {
            get { return _methodName; }
        }

        private readonly IfStatement _polymorphIfStatement;
        private readonly SemanticModel _semanticModel;
        public IfStatement PolymorphIfStatement
        {
            get { return _polymorphIfStatement; }
        }

        public PolymorphMethod(string methodName, SemanticModel semanticModel)
        {
            _methodName = methodName;
            _semanticModel = semanticModel;
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
                var vbl = parameterSyntax.Identifier.ValueText.Variable(ArgumentsVal.ToIdent().Index(i));
                cb.CollectSymbol(vbl);
                i++;
            }
        }

        private ParenthesisExpression BuildTypeCondition(ParameterListSyntax parameters)
        {
            FactParameterList argumentsMathParams = new FactParameterList();
            argumentsMathParams.CollectSymbol(ArgumentsVal.ToIdent());
            
            foreach (var parameterSyntax in parameters.Parameters)
            {
                var symbolInfo = CSharpExtensions.GetSymbolInfo(_semanticModel,parameterSyntax.Type);
                var type = (ITypeSymbol) symbolInfo.Symbol;
                var name = type.FullQualifiedName().Literal();
                argumentsMathParams.CollectSymbol(name);
            }
            var condition = JavascriptElements.MatchArgumentsFunction.ToIdent().Call(argumentsMathParams).Parenthesize();
            return condition;
        }
    }
}
