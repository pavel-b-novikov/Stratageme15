using System;
using System.Collections.Generic;
using Stratageme15.Core.Compiler.Exceptions;
using Stratageme15.Core.JavascriptCodeDom;

namespace Stratageme15.Core.Compiler
{
    public class NodeCompilersRepository
    {
        private Dictionary<Type, INodeCompiler> _nodes;

        public NodeCompilersRepository()
        {
            _nodes = new Dictionary<Type, INodeCompiler>();
        }

        public void Register<TNodeCompiler>() where TNodeCompiler : INodeCompiler, new()
        {
            var nc = new TNodeCompiler();
            nc.Repository = this;
            var tp = typeof(TNodeCompiler);
            Add(nc, tp);
        }
        private void Add(INodeCompiler nc, Type nodeCompilerType)
        {
            var tokenType = ParentOfType(nodeCompilerType,typeof(NodeCompilerBase<>)).GetGenericArguments()[0];
            _nodes[tokenType] = nc;
        }

        private Type ParentOfType(Type source, Type desiredParent)
        {
            while (source!=null&&(source.GUID!=desiredParent.GUID))
            {
                source = source.BaseType;
            }
            return source;
        }
        public void Register(Type nodeCompilerType)
        {
            if (!typeof(INodeCompiler).IsAssignableFrom(nodeCompilerType))
                throw new NodeCompilerRegistrationException(nodeCompilerType,
                                                            "registering node does not implement INodeCompiler");
            var nc = (INodeCompiler)Activator.CreateInstance(nodeCompilerType);
            nc.Repository = this;
            var tp = nc.GetType();
            Add(nc, tp);
        }

        public NodeCompilerBase<TToken> Get<TToken>() where TToken : SyntaxTreeNodeBase
        {
            var tp = typeof(TToken);
            return (NodeCompilerBase<TToken>) Get(tp);
        }

        public INodeCompiler Get(Type tokenType)
        {
            var t = tokenType;
            while (t != null && (!_nodes.ContainsKey(t)))
            {
                t = t.BaseType;
            }
            if (t==null) throw new NoSuitableNodeCompiler(tokenType);
            return _nodes[t];
        }

        public void Clear()
        {
            _nodes.Clear();
        }
    }
}
