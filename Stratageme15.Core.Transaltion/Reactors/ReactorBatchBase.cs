using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Stratageme15.Core.Transaltion.Reactors
{
    public abstract class ReactorBatchBase
    {
        //(reactor,node)
        private readonly List<Tuple<Type, Type>> _reactorTypes;
        private bool _isReactorsListInitialized;

        protected ReactorBatchBase()
        {
            _reactorTypes = new List<Tuple<Type, Type>>();
            _isReactorsListInitialized = false;
        }

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

        public abstract object ReactorBatchData { get; }

        protected void RegisterReactor<TReactor, TNode>() where TNode : SyntaxNode where TReactor : IReactor
        {
            _reactorTypes.Add(new Tuple<Type, Type>(typeof (TReactor), typeof (TNode)));
        }

        protected abstract void Reactors();
    }
}