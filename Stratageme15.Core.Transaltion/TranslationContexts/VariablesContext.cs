using System;
using System.Collections.Generic;
using System.Linq;

namespace Stratageme15.Core.Transaltion.TranslationContexts
{
    public class VariablesContext
    {
        private readonly LinkedList<VariableDescription> _methodParameters = new LinkedList<VariableDescription>();
        private readonly Stack<VariableDescription> _variablesStack = new Stack<VariableDescription>();

        public IEnumerable<VariableDescription> MethodParameters { get { return _methodParameters; } }
        public IEnumerable<VariableDescription> CurrentBlockVariables { get { return _variablesStack.Where(c => c.StackLevel == _currentStackLevel); } }
        public IEnumerable<VariableDescription> AllVariables
        {
            get
            {
                foreach (var variableDescription in _methodParameters)
                {
                    yield return variableDescription;
                }

                foreach (var variableDescription in _variablesStack)
                {
                    yield return variableDescription;
                }
            }
        }

        private int _currentStackLevel;

        public void PushMethodParameter(string parameterName, Type parameterType)
        {
            var descr = new VariableDescription(parameterType, parameterName, true);
            _methodParameters.AddLast(descr);
        }

        public void DefineVariable(string name, Type variableType)
        {
            VariableDescription vd = new VariableDescription(variableType, name, false, _currentStackLevel);
            _variablesStack.Push(vd);
        }
        public void PushContext()
        {
            _currentStackLevel++;
        }

        public void PopContext()
        {
            if (_currentStackLevel - 1 < 0) throw new Exception("Variables stack violation");
            _currentStackLevel--;
            if (_variablesStack.Count == 0) return;
            while (_variablesStack.Count > 0 && _variablesStack.Peek().StackLevel >= _currentStackLevel)
            {
                _variablesStack.Pop();
            }
        }

        #region Type promisement

        public bool IsTypePromised { get; private set; }
        private LinkedList<VariableDescription> _promiseesAwaitingType = new LinkedList<VariableDescription>();
        public void PromiseType()
        {
            IsTypePromised = true;
        }
        public void ResolveTypeForPromisees(Type variableType)
        {
            foreach (var variableDescription in _promiseesAwaitingType)
            {
                variableDescription.VariableType = variableType;
                _variablesStack.Push(variableDescription);
            }
            _promiseesAwaitingType.Clear();
            IsTypePromised = false;
        }

        private Type _nextDeclarationsType;
        public bool IsNextDeclarationsTypeSet { get { return _nextDeclarationsType != null; } }
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
                VariableDescription vd = new VariableDescription(null, name, false, _currentStackLevel);
                _promiseesAwaitingType.AddLast(vd);
                return;
            }

            if (_nextDeclarationsType==null) throw new Exception("Before using Declare without type please call StartDeclaringWithType");
            DefineVariable(name,_nextDeclarationsType);
        }
        #endregion
    }
}