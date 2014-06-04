using System;
using System.Collections.Generic;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom;

namespace Stratageme15.Core.Transaltion.Reactors
{
    public abstract class ReactorBatchBase
    {
        //(reactor,node)
        private readonly List<Tuple<Type,Type>> _reactorTypes;
        private bool _isReactorsListInitialized;
        public IEnumerable<Tuple<Type, Type>> ReactorTypes
        {
            get
            {
                if (!_isReactorsListInitialized)
                {
                    Reactors();
                    _isReactorsListInitialized = true;
                }
                return _reactorTypes;
            }
        }

        protected ReactorBatchBase()
        {
            _reactorTypes = new List<Tuple<Type, Type>>();
            _isReactorsListInitialized = false;
        }

        protected void RegisterReactor<TReactor,TNode>() where TNode : SyntaxNode where TReactor : IReactor
        {
            _reactorTypes.Add(new Tuple<Type, Type>(typeof(TReactor),typeof(TNode)));
        }

        protected abstract void Reactors();


    }
}
