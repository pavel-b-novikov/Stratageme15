using System;
using Microsoft.CodeAnalysis;
namespace Stratageme15.Reactors.Basic.Contexts
{
    public class VariableDescription
    {
        internal VariableDescription(TypeInfo variableType, string variableName, bool isFunctionParameter)
        {
            VariableType = variableType;
            VariableName = variableName;
            IsFunctionParameter = isFunctionParameter;
            StackLevel = 0;
        }

        internal VariableDescription(TypeInfo variableType, string variableName, bool isFunctionParameter, int stackLevel)
        {
            VariableType = variableType;
            VariableName = variableName;
            IsFunctionParameter = isFunctionParameter;
            StackLevel = stackLevel;
        }

        internal VariableDescription(string variableName, bool isFunctionParameter)
        {
            VariableName = variableName;
            IsFunctionParameter = isFunctionParameter;
            StackLevel = 0;
        }

        internal VariableDescription(string variableName, bool isFunctionParameter, int stackLevel)
        {
            VariableName = variableName;
            IsFunctionParameter = isFunctionParameter;
            StackLevel = stackLevel;
        }

        public TypeInfo VariableType { get; internal set; }
        public string VariableName { get; private set; }
        public bool IsFunctionParameter { get; private set; }
        internal int StackLevel { get; private set; }
    }
}