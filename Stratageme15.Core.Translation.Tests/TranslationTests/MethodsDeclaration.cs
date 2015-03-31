using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stratageme15.Reactors.Basic.Tests.TranslationTests
{
    [TestClass]
    public class MethodsDeclaration : BasicBatchTestBase
    {
        [TestMethod]
        public void SimpleVoidPublicMethod()
        {
            AssertTranslated(
    @"
            public class MyClass {
                public void DoSomething(){

                }
            }
            ", J.Class(TestClassName, J.Constructor(TestClassName)
                .With(J.Method(TestClassName,"DoSomething"))
                ).Colon()
            );
        }

        [TestMethod]
        public void SimplePublicMethod()
        {
            AssertTranslated(
    @"
            public class MyClass {
                public int DoSomething(){
                    return 0;
                }
            }
            ", J.Class(TestClassName, J.Constructor(TestClassName)
                .With(J.Method(TestClassName, "DoSomething",body:"return 0;"))
                ).Colon()
            );
        }

        [TestMethod]
        public void SimplePublicStaticMethod()
        {
            AssertTranslated(
    @"
            public class MyClass {
                public static int DoSomething(){
                    return 0;
                }
            }
            ", J.Class(TestClassName, J.Constructor(TestClassName)
                .With(J.StaticMethod(TestClassName, "DoSomething", body: "return 0;"))
                ).Colon()
            );
        }

        [TestMethod]
        public void SimplePrivateMethod()
        {
            AssertTranslated(
    @"
            public class MyClass {
                private int DoSomething(){
                    return 0;
                }
            }
            ", J.Class(TestClassName, J.Constructor(TestClassName)
                .With(J.PrivateMethod("DoSomething", body: "return 0;"))
                ).Colon()
            );
        }

        [TestMethod]
        public void SimplePrivateStaticMethod()
        {
            // yes, it's equals to simple static method
            // sad but true
            // seriously, I have no idea how to implement 
            // private method without usage of "this" qualifier
            AssertTranslated(
    @"
            public class MyClass {
                private static int DoSomething(){
                    return 0;
                }
            }
            ", J.Class(TestClassName, J.Constructor(TestClassName)
                .With(J.PrivateMethod("DoSomething", body: "return 0;"))
                ).Colon()
            );
        }
    }
}
