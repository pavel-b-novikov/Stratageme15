using System;

namespace Stratageme15.Core.Translation.TranslationContexts
{
    public class VariableDescription
    {
        internal VariableDescription(Type variableType, string variableName, bool isFunctionParameter)
        {
            VariableType = variableType;
            VariableName = variableName;
            IsFunctionParameter = isFunctionParameter;
            StackLevel = 0;
        }

        internal VariableDescription(Type variableType, string variableName, bool isFunctionParameter, int stackLevel)
        {
            VariableType = variableType;
            VariableName = variableName;
            IsFunctionParameter = isFunctionParameter;
            StackLevel = stackLevel;
        }

        public Type VariableType { get; internal set; }
        public string VariableName { get; private set; }
        public bool IsFunctionParameter { get; private set; }
        internal int StackLevel { get; private set; }
    }
}