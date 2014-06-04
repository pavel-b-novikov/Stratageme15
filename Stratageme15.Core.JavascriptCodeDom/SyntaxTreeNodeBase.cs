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
    /// <summary>
    /// Syntax tree node base. provides basic functionality for Javascript syntax tree nodes.
    /// All javascript syntax tree nodes are derived from this class
    /// </summary>
    public abstract class SyntaxTreeNodeBase : IHierarchical
    {
        /// <summary>
        /// Information about symbol role describing where it was collected
        /// Must not affect compilation/translation process
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Helper method for collecting syntax nodes in corresponding properties
        /// After successfully call of Collect, the property cpecified with "child" parameter contains casted collected node
        /// </summary>
        /// <typeparam name="TThis">Type of node performing collection</typeparam>
        /// <typeparam name="TChild">Type of node being collected</typeparam>
        /// <typeparam name="TIfNodeType">Only perform collection if symbol is of this node type</typeparam>
        /// <param name="child">Expression speciying property in which the result should be placed</param>
        /// <param name="symbol">Symbol is being collected</param>
        /// <returns>Result of collection. True if symbol was successfullt collected, False otherwise</returns>
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

        /// <summary>
        /// Shortcut for Collect allowing not to specify TIfNodeType directly assuming TIfNodeType = TChild
        /// </summary>
        /// <typeparam name="TThis">Type of node performing collection</typeparam>
        /// <typeparam name="TChild">Type of node being collected</typeparam>
        /// <param name="child">Expression speciying property in which the result should be placed</param>
        /// <param name="symbol">Symbol is being collected</param>
        /// <returns>Result of collection. True if symbol was successfullt collected, False otherwise</returns>
        protected bool CollectExact<TThis,TChild>(Expression<Func<TThis, TChild>> child, SyntaxTreeNodeBase symbol) where TChild:SyntaxTreeNodeBase
        {
            return Collect<TThis, TChild, TChild>(child, symbol);
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }

        public SyntaxTreeNodeBase Parent { get; set; }
        
        /// <summary>
        /// Wraps symbol in CodeBlock if it is statement
        /// </summary>
        /// <param name="symbol">Syntax node</param>
        protected void WrapIfStatement(ref SyntaxTreeNodeBase symbol)
        {
            if (Is<IStatement>(symbol))
            {
                symbol = symbol.WrapInCodeBlock();
            }
        }

        /// <summary>
        /// In overriden class performs the collection.
        /// Collection means taking passed symbol (Syntax Node) and attaching it as children node and placing it in corresponding property of syntax node
        /// </summary>
        /// <param name="symbol">Syntax node is being collected</param>
        public virtual void CollectSymbol(SyntaxTreeNodeBase symbol)
        {
            if (!TryAutoCollectSymbolViaReflection(symbol)) throw new UnexpectedException(symbol,this);
        }

        /// <summary>
        /// Tries automatically collect symbol if there isonly one property of symbol's type present
        /// </summary>
        /// <param name="symbol">Syntax tree node</param>
        /// <returns>True if collection succeeded. False otherwise.</returns>
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

        protected abstract IEnumerable<SyntaxTreeNodeBase> EnumerateChildNodes();


        public IEnumerable<SyntaxTreeNodeBase> Children { get { return EnumerateChildNodes(); } }

        public bool IsScolonNeeded { get; set; }

    }
}
