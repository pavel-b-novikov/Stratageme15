using System;
using System.Collections.Generic;
using Stratageme15.Core.Transaltion.Extensions;
using Stratageme15.ExtensionsPack.Tactics.Base;

namespace Stratageme15.ExtensionsPack.Tactics
{
    public class TacticsRepository
    {
        private readonly Dictionary<Type, ClassTranslationTacticsBase> _tacticsForTypes;
        private readonly ClassTranslationTacticsBase _defaultTactics;
        private readonly ClassTranslationTacticsBase[] _defaultsList;
        
        public TacticsRepository(ClassTranslationTacticsBase defaultTactics)
        {
            _tacticsForTypes = new Dictionary<Type, ClassTranslationTacticsBase>();
            _defaultTactics = defaultTactics;
            _defaultsList = new[] { _defaultTactics };
        }

        public void RegisterTactics<TType,TTactics>() where TTactics : ClassTranslationTacticsBase
        {
            RegisterTactics(typeof(TType),typeof(TTactics));
        }

        public void RegisterTactics(Type typeFor,Type typeOfTactics)
        {
            RegisterTactics(typeFor,(ClassTranslationTacticsBase)Activator.CreateInstance(typeOfTactics));
        }

        public void RegisterTactics(Type typeFor, ClassTranslationTacticsBase tactics)
        {
          // GetOrCreateTacticsList(typeFor).AddLast(tactics);
        }

        public IEnumerable<ClassTranslationTacticsBase> GetAppropriateTactics(Type typeFor)
        {
            //if (!_tacticsForTypes.ContainsKey(typeFor)) return _defaultsList;
            //return _tacticsForTypes[typeFor];
            return null; //todo
        }

        public ClassTranslationTacticsBase DefaultTactics { get { return _defaultTactics; } }
    }
}
