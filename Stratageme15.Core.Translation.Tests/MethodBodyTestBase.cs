using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratageme15.Reactors.Basic.Tests
{
    public class MethodBodyTestBase : BasicBatchTestBase
    {
        protected static string CSharpPublicInstanceMethodBody(string body,string returnType = "void")
        {
            return string.Format(
                @"
            public class {1} {{
                public {2} DoSomething(){{
                    {0}
                }}
            }}
            ", body, TestClassName,returnType);
        }

        protected static string JsPublicInstanceMethodBody(string body)
        {
            return J.Class(TestClassName, J.Constructor(TestClassName)
                .With(J.Method(TestClassName, "DoSomething",
                    body: body
                    ))
                ).Colon();
        }

        protected static string CSharpPrivateInstanceMethodBody(string body, string returnType = "void")
        {
            return string.Format(
                @"
            public class {1} {{
                private {2} DoSomething(){{
                    {0}
                }}
            }}
            ", body, TestClassName,returnType);
        }

        protected static string JsPrivateInstanceMethodBody(string body)
        {
            return J.Class(TestClassName, J.Constructor(TestClassName)
                .With(J.PrivateMethod("DoSomething",
                    body: body
                    ))
                ).Colon();
        }

        protected static string CSharpPrivateStaticMethodBody(string body, string returnType = "void")
        {
            return string.Format(
                @"
            public class {1} {{
                private static {2} DoSomething(){{
                    {0}
                }}
            }}
            ", body, TestClassName,returnType);
        }

        protected static string JsPrivateStaticMethodBody(string body)
        {
            return J.Class(TestClassName, J.Constructor(TestClassName)
                .With(J.PrivateMethod("DoSomething",
                    body: body
                    ))
                ).Colon();
        }

        protected static string CSharpPublicStaticMethodBody(string body, string returnType = "void")
        {
            return string.Format(
                @"
            public class {1} {{
                public static {2} DoSomething(){{
                    {0}
                }}
            }}
            ", body, TestClassName, returnType);
        }

        protected static string JsPublicStaticMethodBody(string body)
        {
            return J.Class(TestClassName, J.Constructor(TestClassName)
                .With(J.StaticMethod(TestClassName, "DoSomething",
                    body: body
                    ))
                ).Colon();
        }

        protected void AssertBodyTranslated(string cs, string js,string message,string returnType = "void")
        {
            AssertTranslated(
                CSharpPublicInstanceMethodBody(cs,returnType),
                JsPublicInstanceMethodBody(js),
                string.Format("Public instance {0} FAILED",message)
                );

            AssertTranslated(
                CSharpPublicStaticMethodBody(cs, returnType),
                JsPublicStaticMethodBody(js),
                string.Format("Public static {0} FAILED", message)
                );

            AssertTranslated(
                CSharpPrivateInstanceMethodBody(cs, returnType),
                JsPrivateInstanceMethodBody(js),
                string.Format("Private instance {0} FAILED", message)
                );

            AssertTranslated(
                CSharpPrivateStaticMethodBody(cs, returnType),
                JsPrivateStaticMethodBody(js),
                string.Format("Private static {0} FAILED", message)
                );
        }


    }
}
