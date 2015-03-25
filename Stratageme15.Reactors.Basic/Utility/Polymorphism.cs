using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Reactors.Basic.Utility
{
    public class Polymorphism
    {
        private readonly TranslationContextWrapper _context;
        private readonly Dictionary<string,PolymorphMethod> _polymorps = new Dictionary<string, PolymorphMethod>();

        public Polymorphism(TranslationContextWrapper context)
        {
            _context = context;
        }

        public void RegisterPolymorphism(string methodName)
        {
            if (_polymorps.ContainsKey(methodName)) return;
            _polymorps.Add(methodName,new PolymorphMethod(methodName,_context));
        }

        public bool LookupPolymorohism(string methodName)
        {
            return _polymorps.ContainsKey(methodName);
        }

        public void PushPolymorphism(string methodName,ParameterListSyntax parameters)
        {
            var poly = _polymorps[methodName];
            var codeBlock = poly.AddOverload(parameters);
            _context.Context.PushContextNode(codeBlock);
        }

        public void PopPolymorphism()
        {
            _context.Context.PopContextNode();
        }

        public IStatement[] EmitPolymorphicMethods(string className)
        {
            List<IStatement> polymorph = new List<IStatement>(_polymorps.Count);
            polymorph.AddRange(_polymorps
                .Select(polymorphMethod => polymorphMethod.Value)
                .Select(pm => JavascriptHelper.CreateEmptyFunction()
                        .AppendStatement(pm.PolymorphIfStatement)
                        .AsMethod(className, pm.MethodName))
                );
            return polymorph.ToArray();
        }
    }
}
