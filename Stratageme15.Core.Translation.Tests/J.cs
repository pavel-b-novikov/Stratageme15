using System;
using System.Linq;
using System.Text;

namespace Stratageme15.Reactors.Basic.Tests
{
    public static class J
    {
        public const string SimpleClassDeclaration = @"
var {0} = (function () {{        
		{1}
        {0}.prototype.__getFullQualifiedName = function () {{ return ""{0}""; }};
        {0}.prototype.__getNamespace = function () {{ return """"; }};
        {0}.__getFullQualifiedName = function () {{ return ""{0}""; }};
        {0}.__getNamespace = function () {{ return """"; }};
        return {0};
    }})();";

        public const string NamespaceClassDeclaration = @"
__namespace(""{1}"", (function () {{        
		{2}
        {0}.prototype.__getFullQualifiedName = function () {{ return ""{1}.{0}""; }};
        {0}.prototype.__getNamespace = function () {{ return ""{1}""; }};
        {0}.__getFullQualifiedName = function () {{ return ""{1}.{0}""; }};
        {0}.__getNamespace = function () {{ return ""{1}""; }};
        return {0};
    }})()
	);";

        public const string SystemJsFunctionsWithNesting = @"
        {0}.prototype.__getFullQualifiedName = function () {{ return ""{2}""; }};
        {0}.prototype.__getNamespace = function () {{ return ""{1}""; }};
        {0}.__getFullQualifiedName = function () {{ return ""{2}""; }};
        {0}.__getNamespace = function () {{ return ""{1}""; }};
";
        public const string SystemJsFunctions = @"
        {0}.prototype.__getFullQualifiedName = function () {{ return ""{1}{2}.{0}""; }};
        {0}.prototype.__getNamespace = function () {{ return ""{1}""; }};
        {0}.__getFullQualifiedName = function () {{ return ""{1}{2}.{0}""; }};
        {0}.__getNamespace = function () {{ return ""{1}""; }};
";
        public const string ConstructorTemplate = @"
        function {0}({1}) {{ {2} }}
";
        public const string MethodTemplate = @"
        {0}.prototype.{1} = function ({2}) {{ {3} }}
";
        public const string StaticMethodTemplate = @"
        {0}.{1} = function ({2}) {{ {3} }} 
";
        public const string NestingTemplate = @"
        {0}.{1} = {1}; 
";
        public const string FunctionTemplate = @"
        function {0} ({1}) {{ {2} }} 
";

        public static string Class(string className, string body = null)
        {
            return String.Format(SimpleClassDeclaration, className,body);
        }

        public static string NsClass(string className, string ns, string body = null)
        {
            return String.Format(NamespaceClassDeclaration, className, ns, body);
        }

        public static string Constructor(string className, string parameters = null, string body = null)
        {
            return String.Format(ConstructorTemplate, className, parameters, body);
        }

        public static string PrivateMethod(string methodName, string parameters = null, string body = null)
        {
            return String.Format(FunctionTemplate, methodName, parameters, body);
        }

        public static string Method(string className, string methodName, string parameters = null, string body = null)
        {
            return String.Format(MethodTemplate, className, methodName, parameters, body);
        }

        public static string StaticMethod(string className, string methodName, string parameters = null, string body = null)
        {
            return String.Format(StaticMethodTemplate, className, methodName, parameters, body);
        }

        public static string Parameters(params string[] parameters)
        {
            return parameters.Aggregate((a, v) => String.Format("{0}, {1}", a, v));
        }

        public static string Nesting(string parentName,string childName)
        {
            return String.Format(NestingTemplate, parentName, childName);
        }

        public static string SystemMethods(string className,string ns)
        {
            return String.Format(SystemJsFunctions, className, ns);
        }

        public static string SystemMethods(string className, string ns,string nester)
        {
            string fullQualifiedWithNester = string.Format("{0}.{1}.{2}", ns, nester, className);
            fullQualifiedWithNester = fullQualifiedWithNester.Trim('.');
            return String.Format(SystemJsFunctionsWithNesting, className, ns, fullQualifiedWithNester);
        }

        public static string Colon(this string s)
        {
            return String.Concat(s, ";");
        }

        public static string With(this string s,string appendent)
        {
            return String.Concat(s, "\n",appendent);
        }
    }
}
