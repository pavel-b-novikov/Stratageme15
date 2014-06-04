using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Stratageme15.Core.JavascriptCodeDom.Exceptions;
using Stratageme15.Core.JavascriptCodeDom.Extensions;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.JavascriptCodeDom
{
    public abstract class SyntaxTreeNodeBase : IHierarchical
    {
        public string Role
        {
            get { return _role; }
            set
            {
                
                //if (!string.IsNullOrEmpty(_role))
                //{
                //    if (_role.Equals(value)) return;
                //    throw new RoleChangedException(this,_role,value);
                //}
                _role = value;
            }
        }

        protected bool Collect<TThis, TChild, TIfNodeType>(Expression<Func<TThis, TChild>> child, SyntaxTreeNodeBase symbol) where TChild:SyntaxTreeNodeBase
        {
            if (Is<TIfNodeType>(symbol))
            {
                PropertyInfo pi = null;
                var body = child.Body;
                if (body is MemberExpression)
                {
                    var mci = (MemberExpression)body;
                    pi = (PropertyInfo) mci.Member;
                }
                var val = pi.GetValue(this);
                if (val==null)
                {
                    symbol.Role = pi.Name;
                    TChild tc = (TChild) symbol;
                    pi.SetValue(this,tc);
                    return true;
                }
            }
            return false;
        }

        protected bool CollectExact<TThis,TChild>(Expression<Func<TThis, TChild>> child, SyntaxTreeNodeBase symbol) where TChild:SyntaxTreeNodeBase
        {
            return Collect<TThis, TChild, TChild>(child, symbol);
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }

        public SyntaxTreeNodeBase Parent { get; set; }
        
        protected void WrapIfStatement(ref SyntaxTreeNodeBase symbol)
        {
            if (Is<IStatement>(symbol))
            {
                symbol = symbol.WrapInCodeBlock();
            }
        }
        public virtual void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (!TryAutoCollectSymbolViaReflection(symbol)) throw new UnexpectedException(symbol,this);
        }

        protected bool TryAutoCollectSymbolViaReflection(SyntaxTreeNodeBase symbol)
        {
            var t = symbol.GetType();
            var self = GetType();

            var props = self.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType==t).ToArray();
            if (props.Length == 1)
            {
                var val = props[0].GetValue(this);
                if (val == null)
                {
                    props[0].SetValue(this, symbol);
                    symbol.Role = props[0].Name;
                    return true;
                }
            }
            return false;
        }

        public virtual void CollectOperator(AssignmentOperator op)
        {
            throw new UnexpectedException(op, this);
        }

        public virtual void CollectOperator(UnaryOperator op)
        {
            throw new UnexpectedException(op, this);
        }

        public virtual void CollectOperator(MathOperator op)
        {
            throw new UnexpectedException(op, this);
        }

        public virtual void CollectOperator(LogicalOperator op)
        {
            throw new UnexpectedException(op, this);
        }

        public virtual void CollectOperator(BitwiseOperator op)
        {
            throw new UnexpectedException(op, this);
        }

        public virtual void CollectOperator(ComparisonOperator op)
        {
            throw new UnexpectedException(op, this);
        }

        public virtual void CollectOperator(IndrementDecrementOperator op)
        {
            throw new UnexpectedException(op, this);
        }

        protected bool Check<T>(SyntaxTreeNodeBase node,object prop)
        {
            return prop == null && node is T;
        }

        protected bool Is<TExpr>(object symbol)
        {
            return typeof (TExpr).IsAssignableFrom(symbol.GetType());
        }

        protected bool Is(object symbol,Type t)
        {
            return t.IsAssignableFrom(symbol.GetType());
        }

        protected static SyntaxTreeNodeBase[] Empty = new SyntaxTreeNodeBase[0];
        private string _role;

        protected abstract IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes();


        public IEnumerable<SyntaxTreeNodeBase> Children { get { return EnumerateChildNodes(); } }

        public bool IsScolonNeeded { get; set; }

        public virtual void CollectSymbolFirst(SyntaxTreeNodeBase symbol)
        {
            
        }
    }
}
