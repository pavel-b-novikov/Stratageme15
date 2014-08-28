using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.CodeAnalysis;
using Stratageme15.Core.Translation.Extensions;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Core.Translation.Repositories
{
    public class ReactorRepository
    {
        private readonly Dictionary<Guid, object> _allReactorsInstancesByReactorTypeId;
        private readonly Dictionary<Guid, LinkedList<IReactor>> _reactorInstances;

        /// <summary>
        /// 1 - syntax node
        /// 2 - reactor type
        /// </summary>
        private readonly Dictionary<Type, LinkedList<Type>> _reactors;

        private readonly Dictionary<Type, object> _reactorsBatchData;
        private readonly Dictionary<Guid, LinkedList<ISituationReactor>> _situationReactorInstances;
        private readonly Dictionary<Type, LinkedList<Type>> _situationReactors;

        public ReactorRepository()
        {
            _reactors = new Dictionary<Type, LinkedList<Type>>();
            _situationReactors = new Dictionary<Type, LinkedList<Type>>();
            _reactorInstances = new Dictionary<Guid, LinkedList<IReactor>>();
            _situationReactorInstances = new Dictionary<Guid, LinkedList<ISituationReactor>>();
            _allReactorsInstancesByReactorTypeId = new Dictionary<Guid, object>();
            _reactorsBatchData = new Dictionary<Type, object>();
        }

        #region Registration API

        public void RegisterCommonReactor(Type tNode, Type tReactor)
        {
            if (!typeof (SyntaxNode).IsAssignableFrom(tNode)) throw new ArgumentException("tNode");

            if (typeof (ISituationReactor).IsAssignableFrom(tReactor))
            {
                _situationReactors.GetOrCreate(tNode).AddLast(tReactor);
            }
            else
            {
                if (typeof (IReactor).IsAssignableFrom(tReactor))
                {
                    _reactors.GetOrCreate(tNode).AddLast(tReactor);
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
            if (batch.ReactorBatchData != null)
            {
                _reactorsBatchData.Add(batch.GetType(), batch.ReactorBatchData);
            }
        }

        public T GetReactorBatchData<T, TReactorBatch>()
            where T : class
            where TReactorBatch : ReactorBatchBase
        {
            Type t = typeof (TReactorBatch);
            if (!_reactorsBatchData.ContainsKey(t)) return null;
            return (T) _reactorsBatchData[t];
        }

        #endregion

        #region Retrievement API

        private static readonly ReadOnlyCollection<IReactor> Empty = new ReadOnlyCollection<IReactor>(new IReactor[0]);

        private static readonly ReadOnlyCollection<ISituationReactor> SituationEmpty =
            new ReadOnlyCollection<ISituationReactor>(new ISituationReactor[0]);

        public IEnumerable<IReactor> GetAllSituationOrDefault(TranslationContext ctx)
        {
            IEnumerable<IReactor> situation = GetSuitableSituationReactors(ctx);
            IEnumerable<IReactor> baseReactor = GetSimpleReactors(ctx.SourceNode.GetType());
            return baseReactor.Concat(situation).Reverse();
        }

        public IEnumerable<IReactor> GetSimpleReactors(Type tNodeType)
        {
            if (!_reactorInstances.ContainsKey(tNodeType.GUID))
            {
                if (!_reactors.ContainsKey(tNodeType)) return Empty;
                LinkedList<Type> reactor = _reactors[tNodeType];
                LinkedList<IReactor> instances = _reactorInstances.GetOrCreate(tNodeType.GUID);
                foreach (Type type in reactor)
                {
                    if (_allReactorsInstancesByReactorTypeId.ContainsKey(type.GUID))
                    {
                        instances.AddLast((IReactor) _allReactorsInstancesByReactorTypeId[type.GUID]);
                    }
                    else
                    {
                        var reactorInstance = (IReactor) Activator.CreateInstance(type);
                        instances.AddLast(reactorInstance);
                        _allReactorsInstancesByReactorTypeId.Add(type.GUID, reactorInstance);
                    }
                }
            }
            return _reactorInstances[tNodeType.GUID];
        }

        public IReactor GetSituationReactor(TranslationContext ctx)
        {
            ReadOnlyCollection<ISituationReactor> allSituationReactors = GetSituationReactors(ctx);

            return allSituationReactors.FirstOrDefault(allSituationReactor => allSituationReactor.IsAcceptable(ctx));
        }

        public IEnumerable<IReactor> GetSuitableSituationReactors(TranslationContext ctx)
        {
            ReadOnlyCollection<ISituationReactor> allSituationReactors = GetSituationReactors(ctx);

            return allSituationReactors.Where(allSituationReactor => allSituationReactor.IsAcceptable(ctx));
        }

        public ReadOnlyCollection<ISituationReactor> GetSituationReactors(TranslationContext ctx)
        {
            Type tNodeType = ctx.SourceNode.GetType();

            LinkedList<ISituationReactor> allSituationReactors;
            if (!_situationReactorInstances.ContainsKey(tNodeType.GUID))
            {
                if (!_situationReactors.ContainsKey(tNodeType))
                {
                    return SituationEmpty;
                }
                LinkedList<Type> potentiallyAppropriate = _situationReactors[tNodeType];
                var l = new LinkedList<ISituationReactor>();

                foreach (Type type in potentiallyAppropriate)
                {
                    ISituationReactor sr;
                    if (_allReactorsInstancesByReactorTypeId.ContainsKey(type.GUID))
                    {
                        sr = (ISituationReactor) _allReactorsInstancesByReactorTypeId[type.GUID];
                    }
                    else
                    {
                        sr = (ISituationReactor) Activator.CreateInstance(type);
                        _allReactorsInstancesByReactorTypeId.Add(type.GUID, sr);
                    }

                    l.AddLast(sr);
                }
                _situationReactorInstances[tNodeType.GUID] = l;
                allSituationReactors = l;
            }
            else
            {
                allSituationReactors = _situationReactorInstances[tNodeType.GUID];
            }
            return new ReadOnlyCollection<ISituationReactor>(new List<ISituationReactor>(allSituationReactors));
        }

        #endregion
    }
}