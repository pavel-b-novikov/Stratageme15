using System.Collections.Generic;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;

namespace Stratageme15.Core.Transaltion.Builders.Expressions
{
    public class MemberAccessExpressionBuilder : ISyntaxBuilder<Expression>
    {
        private readonly Stack<Expression> _accessStack = new Stack<Expression>();
        private readonly Expression _baseExpression;

        public MemberAccessExpressionBuilder(string ident)
        {
            _baseExpression = ident.Ident();
        }

        public MemberAccessExpressionBuilder(Expression baseExpression)
        {
            _baseExpression = baseExpression;
        }

        #region ISyntaxBuilder<Expression> Members

        public Expression Build()
        {
            Expression top = _accessStack.Pop();
            Expression cExp = top;
            while (_accessStack.Count > 0)
            {
                Expression postfix = _accessStack.Pop();
                cExp.CollectSymbol(postfix);
                cExp = postfix;
            }
            if (_baseExpression != null)
            {
                cExp.CollectSymbol(_baseExpression);
            }
            return top;
        }

        #endregion

        public MemberAccessExpressionBuilder Field(string fieldName)
        {
            _accessStack.Push(new FieldAccessExpression {Member = fieldName.Ident()});
            return this;
        }

        public MemberAccessExpressionBuilder Call(string methodName = null, FactParameterList factParameters = null)
        {
            if (!string.IsNullOrWhiteSpace(methodName))
            {
                Field(methodName);
            }
            _accessStack.Push(new CallExpression {Parameters = factParameters});
            return this;
        }
    }
}