using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Logging;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.Repositories;

namespace Stratageme15.Reactors.Basic.Tests
{
    public static class StringExtensions
    {
        public static string Colon(this string s)
        {
            return string.Concat(s, ";");
        }
        public static string With(this string s,string appendent)
        {
            return string.Concat(s, "\n",appendent);
        }
    }
    public class BasicBatchTestBase : StratagemeTestBase
    {
        [TestInitialize]
        public void Init()
        {
            base.Init();
        }

        public string CurrentCode { get; private set; }

        private JsProgram Translate(string code)
        {
            return Translate(code, new BasicReactorBatch());
        }

        public Translator InitializeTranslator(string code,out SyntaxTree synTree)
        {
            return InitializeTranslatorWithContext(code, out synTree, new BasicReactorBatch());
        }

#region Test templates

        private const string SimpleClassDeclaration = @"
var {0} = (function () {{        
		{1}
        {0}.prototype.__getFullQualifiedName = function () {{ return ""{0}""; }};
        {0}.prototype.__getNamespace = function () {{ return """"; }};
        {0}.__getFullQualifiedName = function () {{ return ""{0}""; }};
        {0}.__getNamespace = function () {{ return """"; }};
        return {0};
    }})();

";
        private const string NamespaceClassDeclaration = @"
__namespace(""{1}"", (function () {{        
		{2}
        {0}.prototype.__getFullQualifiedName = function () {{ return ""{1}.{0}""; }};
        {0}.prototype.__getNamespace = function () {{ return ""{1}""; }};
        {0}.__getFullQualifiedName = function () {{ return ""{1}.{0}""; }};
        {0}.__getNamespace = function () {{ return ""{1}""; }};
        return {0};
    }})()
	);
";
        private const string SystemJsFunctionsWithNesting = @"
{0}.prototype.__getFullQualifiedName = function () {{ return ""{1}.{0}""; }};
{0}.prototype.__getNamespace = function () {{ return ""{1}""; }};
{0}.__getFullQualifiedName = function () {{ return ""{1}.{0}""; }};
{0}.__getNamespace = function () {{ return ""{1}""; }};
";
        private const string SystemJsFunctions = @"
{0}.prototype.__getFullQualifiedName = function () {{ return ""{1}{2}.{0}""; }};
{0}.prototype.__getNamespace = function () {{ return ""{1}""; }};
{0}.__getFullQualifiedName = function () {{ return ""{1}{2}.{0}""; }};
{0}.__getNamespace = function () {{ return ""{1}""; }};
";
        private const string ConstructorTemplate = @"
function {0}({1}) {{
	{2}
}}
";
        private const string MethodTemplate = @"
{0}.prototype.{1} = function ({2}) {{
	{3}
}}
";
        private const string NestingTemplate = @"
        {0}.{1} = {1};
        ";
        protected string Class(string className, string body = null)
        {
            return string.Format(SimpleClassDeclaration, className,body);
        }
        protected string NsClass(string className, string ns, string body = null)
        {
            return string.Format(NamespaceClassDeclaration, className, ns, body);
        }
        protected string Constructor(string className, string parameters = null, string body = null)
        {
            return string.Format(ConstructorTemplate, className, parameters, body);
        }

        protected string Method(string className, string methodName, string parameters = null, string body = null)
        {
            return string.Format(MethodTemplate, className, methodName, parameters, body);
        }
        protected string Parameters(params string[] parameters)
        {
            return parameters.Aggregate((a, v) => string.Format("{0}, {1}", a, v));
        }
        protected string Nesting(string parentName,string childName)
        {
            return string.Format(NestingTemplate, parentName, childName);
        }
        protected string SystemMethods(string className,string ns)
        {
            return string.Format(SystemJsFunctions, className, ns);
        }
        protected string SystemMethods(string className, string ns,string nester)
        {
            return string.Format(SystemJsFunctionsWithNesting, className, ns, nester);
        }
#endregion
        protected void AssertTranslated(string csCodeSource, string jsCodeExpected)
        {
            var expected = Parse(jsCodeExpected);
            var actual = Translate(csCodeSource);
            CurrentCode = Code(actual);
            Assert.IsTrue(Compare(actual, expected));
        }

    }
}
