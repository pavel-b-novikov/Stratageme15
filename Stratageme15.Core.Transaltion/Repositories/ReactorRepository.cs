using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Core.Transaltion.Repositories
{
    public class ReactorRepository
    {
        /// <summary>
        /// 1 - syntax node
        /// 2 - reactor type
        /// </summary>
        private readonly Dictionary<Type, Type> _reactors;
        private readonly Dictionary<Type, List<Type>> _situationReactors;
        private readonly Dictionary<Guid, IReactor> _reactorInstances;
        private readonly Dictionary<Guid, List<ISituationReactor>> _situationReactorInstances;
        private readonly Dictionary<Guid, object> _allReactorsInstancesByReactorTypeId;
        public ReactorRepository()
        {
            _reactors = new Dictionary<Type, Type>();
            _situationReactors = new Dictionary<Type, List<Type>>();
            _reactorInstances = new Dictionary<Guid, IReactor>();
            _situationReactorInstances = new Dictionary<Guid, List<ISituationReactor>>();
            _allReactorsInstancesByReactorTypeId = new Dictionary<Guid, object>();
        }


        #region Registration API
        public void RegisterCommonReactor(Type tNode, Type tReactor)
        {
            if (!typeof(SyntaxNode).IsAssignableFrom(tNode)) throw new ArgumentException("tNode");

            if (typeof(ISituationReactor).IsAssignableFrom(tReactor))
            {
                if (!_situationReactors.ContainsKey(tNode))
                {
                    _situationReactors[tNode] = new List<Type>();
                }
                _situationReactors[tNode].Add(tReactor);
            }
            else
            {
                if (typeof(IReactor).IsAssignableFrom(tReactor))
                {
                    _reactors[tNode] = tReactor;
                }
                else
                {
                    throw new ArgumentException("tReactor");
                }
            }

        }

        public void RegisterBatch(ReactorBatchBase batch)
        {
            foreach (var reactorType in batch.ReactorTypes)
            {
                RegisterCommonReactor(reactorType.Item2, reactorType.Item1);
            }
        }

        #endregion

        #region Retrievement API
        public IReactor GetSituationOrDefault(TranslationContext ctx)
        {
            var situation = GetSituationReactor(ctx);
            if (situation != null) return situation;
            situation = GetReactor(ctx.SourceNode.GetType());
            return situation;
        }

        public IReactor GetReactor(Type tNodeType)
        {
            if (!_reactorInstances.ContainsKey(tNodeType.GUID))
            {
                if (!_reactors.ContainsKey(tNodeType)) return null; //todo exception?
                var reactor = _reactors[tNodeType];

                var reactorInstance = (IReactor)Activator.CreateInstance(reactor);
                _reactorInstances[tNodeType.GUID] = reactorInstance;
                _allReactorsInstancesByReactorTypeId.Add(reactor.GUID, reactorInstance);
            }
            return _reactorInstances[tNodeType.GUID];
        }

        public IReactor GetSituationReactor(TranslationContext ctx)
        {
            var allSituationReactors = GetSituationReactors(ctx);
            
            return allSituationReactors.FirstOrDefault(allSituationReactor => allSituationReactor.IsAcceptable(ctx));
        }

        public ReadOnlyCollection<ISituationReactor> GetSituationReactors(TranslationContext ctx)
        {
            var tNodeType = ctx.SourceNode.GetType();

            List<ISituationReactor> allSituationReactors;
            if (!_situationReactorInstances.ContainsKey(tNodeType.GUID))
            {
                if (!_situationReactors.ContainsKey(tNodeType)) {
                    allSituationReactors = new List<ISituationReactor>();
                    return new ReadOnlyCollection<ISituationReactor>(allSituationReactors); 
                }
                var potentiallyAppropriate = _situationReactors[tNodeType];
                var l = new List<ISituationReactor>();

                foreach (var type in potentiallyAppropriate)
                {
                    ISituationReactor sr;
                    if (_allReactorsInstancesByReactorTypeId.ContainsKey(type.GUID))
                    {
                        sr = (ISituationReactor)_allReactorsInstancesByReactorTypeId[type.GUID];
                    }
                    else
                    {
                        sr = (ISituationReactor)Activator.CreateInstance(type);
                        _allReactorsInstancesByReactorTypeId.Add(type.GUID, sr);
                    }

                    l.Add(sr);

                }
                _situationReactorInstances[tNodeType.GUID] = l;
                allSituationReactors = l;
            }
            else
            {
                allSituationReactors = _situationReactorInstances[tNodeType.GUID];
            }
            return new ReadOnlyCollection<ISituationReactor>(allSituationReactors);
        }
        #endregion

    }
}
