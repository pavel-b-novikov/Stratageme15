using System;
using System.Collections.Generic;
using System.Linq;
using Stratageme15.Core.Translation.Logging;
using Stratageme15.Core.Translation.TranslationContexts;
using Microsoft.CodeAnalysis;
namespace Stratageme15.Reactors.Basic.Contexts
{
    /// <summary>
    /// Local variables context
    /// Holds information about all local variables and their types defined in corresponding block
    /// with ability to access local variables in parent blocks.
    /// Type inference implicitly works according to variables context
    /// </summary>
    public class VariablesContext
    {
        /// <summary>
        /// Constructs new variables context
        /// </summary>
        /// <param name="context">Current translation context</param>
        public VariablesContext(TranslationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Reference to translation context
        /// </summary>
        private readonly TranslationContext _context;

        /// <summary>
        /// Variables passed as method parameters.
        /// They automatically appear as local variables
        /// </summary>
        private readonly LinkedList<VariableDescription> _methodParameters = new LinkedList<VariableDescription>();

        /// <summary>
        /// Local variables description storage
        /// Formed as stack to perform Push/Pop conect more easily
        /// </summary>
        private readonly Stack<VariableDescription> _variablesStack = new Stack<VariableDescription>();

        /// <summary>
        /// Counter for level of current block syntax
        /// Method { 
        ///     /* _currentStackLevel = 0 */
        ///     for(...){ 
        ///         /* _currentStackLevel = 1 */
        ///         if (...) { 
        ///             /* _currentStackLevel = 2 */
        ///         }
        ///     }
        /// }
        /// </summary>
        private int _currentStackLevel;

        /// <summary>
        /// Index on all variables including variables defined in method parameters and parent blocks
        /// </summary>
        private Dictionary<string, VariableDescription> _allVariables = new Dictionary<string, VariableDescription>();

        /// <summary>
        /// Enumerates all current method parameters
        /// </summary>
        public IEnumerable<VariableDescription> MethodParameters
        {
            get { return _methodParameters; }
        }

        /// <summary>
        /// Enumerates all variables in current block syntax only
        /// </summary>
        public IEnumerable<VariableDescription> CurrentBlockVariables
        {
            get { return _variablesStack.Where(c => c.StackLevel == _currentStackLevel); }
        }

        /// <summary>
        /// Enumerates all currently known local variables including variables defined within ancestor syntax block
        /// </summary>
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

        /// <summary>
        /// Store method parameter in local variables context
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="parameterType">Parameter type</param>
        public void PushMethodParameter(string parameterName, TypeInfo parameterType)
        {
            var descr = new VariableDescription(parameterType, parameterName, true);
            _methodParameters.AddLast(descr);
        }

        /// <summary>
        /// Save declared local variable in variables context
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <param name="variableType">Variable type</param>
        public void DefineVariable(string name, TypeInfo variableType)
        {
            var vd = new VariableDescription(variableType, name, false, _currentStackLevel);
            _variablesStack.Push(vd);
            _allVariables[vd.VariableName] = vd;
        }

        /// <summary>
        /// Creates new variable context block inside existing context
        /// Appears when new block syntax is met in existing block syntax
        /// e.g. in for operator
        /// This method should be called when open brace is met
        /// </summary>
        public void PushContext()
        {
            _currentStackLevel++;
        }

        /// <summary>
        /// Destroys variables defined in inner block syntax
        /// This method should be called when closing brace is met
        /// </summary>
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

        /// <summary>
        /// Storage for type inferenced variables until type is specified
        /// </summary>
        private readonly LinkedList<VariableDescription> _promiseesAwaitingType = new LinkedList<VariableDescription>();

        /// <summary>
        /// Stores type specified for multiple variables
        /// e.g. string a,b,c;
        /// </summary>
        private TypeInfo? _nextDeclarationsType;

        /// <summary>
        /// Are there any variables awaiting type specification
        /// </summary>
        public bool IsTypePromised { get; private set; }

        /// <summary>
        /// Is there type for upcoming variables specified
        /// </summary>
        public bool IsNextDeclarationsTypeSet
        {
            get { return _nextDeclarationsType != null; }
        }

        /// <summary>
        /// Notify variables context about beginning of type-inferred variables declaration
        /// </summary>
        public void PromiseType()
        {
            IsTypePromised = true;
        }

        /// <summary>
        /// Specify type of all variables being defined after PromiseType call
        /// </summary>
        /// <param name="variableType">Resolved type</param>
        public void ResolveTypeForPromisees(TypeInfo variableType)
        {
            if (!IsTypePromised) _context.Crit("Error while declaring type-inferenced variables. PromiseType() must be called before ResolveTypeforPromises call");
            foreach (VariableDescription variableDescription in _promiseesAwaitingType)
            {
                variableDescription.VariableType = variableType;
                _variablesStack.Push(variableDescription);
                _allVariables[variableDescription.VariableName] = variableDescription;
            }
            _promiseesAwaitingType.Clear();
            IsTypePromised = false;
        }

        /// <summary>
        /// Notify variables context of beginning declaration of multiple variables with single type
        /// e.g. string a,b,c;
        /// </summary>
        /// <param name="t"></param>
        public void StartDeclaringWithType(TypeInfo t)
        {
            _nextDeclarationsType = t;
        }

        /// <summary>
        /// Notify variables context of stopping declaration of multiple variables with single type
        /// </summary>
        public void StopDeclaringWithType()
        {
            _nextDeclarationsType = null;
        }

        /// <summary>
        /// Defines variable in case of type promisement enabled or single type for multiple variables specified
        /// Invokation of this method will result an exception in all other cases
        /// </summary>
        /// <param name="name"></param>
        public void DefineVariable(string name)
        {
            if (IsTypePromised)
            {
                var vd = new VariableDescription(name, false, _currentStackLevel);
                _promiseesAwaitingType.AddLast(vd);
                return;
            }

            if (_nextDeclarationsType == null)
            {
                _context.Crit("Before using Declare without type please call StartDeclaringWithType");
                return;
            }
            DefineVariable(name, _nextDeclarationsType.Value);
        }

        /// <summary>
        /// Look up all currently defined variables and return declaration of variable with specified name
        /// </summary>
        /// <param name="name">Variable name to lookup</param>
        /// <returns>Corresponding variable description</returns>
        public VariableDescription GetVariable(string name)
        {
            if (!_allVariables.ContainsKey(name)) return null;
            return _allVariables[name];
        }
        #endregion
    }
}