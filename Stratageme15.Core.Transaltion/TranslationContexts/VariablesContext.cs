using System;
using System.Collections.Generic;
using System.Linq;
using Stratageme15.Core.Translation.Logging;

namespace Stratageme15.Core.Translation.TranslationContexts
{
    public class VariablesContext
    {
        private readonly TranslationContext _context;
        private readonly LinkedList<VariableDescription> _methodParameters = new LinkedList<VariableDescription>();
        private readonly Stack<VariableDescription> _variablesStack = new Stack<VariableDescription>();
        private int _currentStackLevel;
        private Dictionary<string, VariableDescription> _allVariables = new Dictionary<string, VariableDescription>();
        public IEnumerable<VariableDescription> MethodParameters
        {
            get { return _methodParameters; }
        }

        public IEnumerable<VariableDescription> CurrentBlockVariables
        {
            get { return _variablesStack.Where(c => c.StackLevel == _currentStackLevel); }
        }

        public IEnumerable<VariableDescription> AllVariables
        {
            get
            {
                foreach (VariableDescription variableDescription in _methodParameters)
                {
                    yield return variableDescription;
                }

                foreach (VariableDescription variableDescription in _variablesStack)
                {
                    yield return variableDescription;
                }
            }
        }

        public void PushMethodParameter(string parameterName, Type parameterType)
        {
            var descr = new VariableDescription(parameterType, parameterName, true);
            _methodParameters.AddLast(descr);
        }

        public void DefineVariable(string name, Type variableType)
        {
            var vd = new VariableDescription(variableType, name, false, _currentStackLevel);
            _variablesStack.Push(vd);
            _allVariables[vd.VariableName] = vd;
        }

        public void PushContext()
        {
            _currentStackLevel++;
        }

        public void PopContext()
        {
            if (_currentStackLevel - 1 < 0)
            {
                _context.Crit("Variables stack violation");
                return;
            }
            _currentStackLevel--;
            if (_variablesStack.Count == 0) return;
            while (_variablesStack.Count > 0 && _variablesStack.Peek().StackLevel >= _currentStackLevel)
            {
                var vd = _variablesStack.Pop();
                _allVariables.Remove(vd.VariableName);
            }
        }

        #region Type promisement

        private readonly LinkedList<VariableDescription> _promiseesAwaitingType = new LinkedList<VariableDescription>();

        private Type _nextDeclarationsType;

        public VariablesContext(TranslationContext context)
        {
            _context = context;
        }

        public bool IsTypePromised { get; private set; }

        public bool IsNextDeclarationsTypeSet
        {
            get { return _nextDeclarationsType != null; }
        }

        public void PromiseType()
        {
            IsTypePromised = true;
        }

        public void ResolveTypeForPromisees(Type variableType)
        {
            foreach (VariableDescription variableDescription in _promiseesAwaitingType)
            {
                variableDescription.VariableType = variableType;
                _variablesStack.Push(variableDescription);
                _allVariables[variableDescription.VariableName] = variableDescription;
            }
            _promiseesAwaitingType.Clear();
            IsTypePromised = false;
        }

        public void StartDeclaringWithType(Type t)
        {
            _nextDeclarationsType = t;
        }

        public void StopDeclaringWithType()
        {
            _nextDeclarationsType = null;
        }

        public void DefineVariable(string name)
        {
            if (IsTypePromised)
            {
                var vd = new VariableDescription(null, name, false, _currentStackLevel);
                _promiseesAwaitingType.AddLast(vd);
                return;
            }

            if (_nextDeclarationsType == null)
                throw new Exception("Before using Declare without type please call StartDeclaringWithType");
            DefineVariable(name, _nextDeclarationsType);
        }

        public VariableDescription GetVariable(string name)
        {
            if (!_allVariables.ContainsKey(name)) return null;
            return _allVariables[name];
        }
        #endregion
    }
}