using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpExtensions = Microsoft.CodeAnalysis.CSharp.CSharpExtensions;

namespace Stratageme15.Reactors.Basic.Tests
{
    [TestClass]
    public class Experiments : BasicBatchTestBase
    {
        [TestMethod]
        public void TypenameBindingExperiment()
        {
            const string code = @"
namespace TestNamespace {
    public class A {
        public void Method(A arg){
        }
    }
}
";
            SyntaxTree str = null;
            var translator = InitializeTranslator(code,out str);
            var comp = translator.TranslationContext.Compilation;
            
            var sem = translator.TranslationContext.SemanticModel;
            var ns = str.GetCompilationUnitRoot().Members[0] as NamespaceDeclarationSyntax;
            var typeSyntax = ns.Members[0] as TypeDeclarationSyntax;
            var methodDeclr = typeSyntax.Members[0] as MethodDeclarationSyntax;
            
            var parameterType = methodDeclr.ParameterList.Parameters[0].Type;
            var ti = CSharpExtensions.GetSymbolInfo(sem, parameterType);
            Assert.IsNotNull(ti);
        }
    }
}
