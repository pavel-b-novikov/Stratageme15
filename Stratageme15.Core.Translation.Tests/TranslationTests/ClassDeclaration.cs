using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stratageme15.Core.Compiler;
using Stratageme15.Core.JavascriptCodeDom;

namespace Stratageme15.Reactors.Basic.Tests.TranslationTests
{
    [TestClass]
    public class ClassDeclaration : BasicBatchTestBase
    {
        private const string TestClassName = "MyClass";
        
        [TestMethod]
        public void SimpleClassDeclaration()
        {
            AssertTranslated(
                @"
            public class MyClass {

            }
            ",
                Class(TestClassName, Constructor(TestClassName)).Colon()
            );
        }

        [TestMethod]
        public void ClassInSimpleNamespace()
        {
            AssertTranslated(
            @"
            namespace MyCompany {
                public class MyClass {

                }
            }
            ",
             NsClass(TestClassName, "MyCompany", Constructor(TestClassName)).Colon()
            );
        }

        [TestMethod]
        public void ClassInComplexNamespace()
        {
            AssertTranslated(
            @"
            namespace MyCompany.MyTechnology {
                public class MyClass {

                }
            }
            ",
            NsClass(TestClassName, "MyCompany.MyTechnology", Constructor(TestClassName)).Colon()
           );
        }

        [TestMethod]
        public void ClassInNestedNamespace()
        {
            AssertTranslated(
            @"
            namespace MyCompany {
                namespace MyTechnology {
                    public class MyClass {

                    }
                }
            }
            ",
            NsClass(TestClassName, "MyCompany.MyTechnology", Constructor(TestClassName)).Colon()
            );
        }

        [TestMethod]
        public void NestedSimpleClassDeclaration()
        {
            AssertTranslated(
            @"
            public class MyClass {
                public class MyNestedClass {

                }
            }
            ",
             Class(TestClassName,
                Constructor("MyNestedClass")
                    .With(SystemMethods("MyNestedClass", null,TestClassName))
                    .With(Constructor(TestClassName)
                        .With(Nesting(TestClassName, "MyNestedClass")))
                  ).Colon()
            );
        }

        [TestMethod]
        public void NestedWithNamespaceSimpleClassDeclaration()
        {
            AssertTranslated(
            @"
            namespace MyCompany {
                public class MyClass {
                    public class MyNestedClass {

                    }
                }
            }
            ",
             NsClass(TestClassName, "MyCompany",
                Constructor("MyNestedClass")
                    .With(SystemMethods("MyNestedClass", "MyCompany", TestClassName))
                    .With(Constructor(TestClassName)
                    .With(Nesting(TestClassName, "MyNestedClass")))
                  ).Colon()
           );
        }

        [TestMethod]
        public void ComplexNestedWithNamespaceSimpleClassDeclaration()
        {
            AssertTranslated(
            @"
            namespace MyCompany {
                namespace MyTechnology {
                    public class MyClass {
                        public class MyNestedClass {

                        }
                    }
                }
            }
            ",
              NsClass(TestClassName, "MyCompany.MyTechnology",
                Constructor("MyNestedClass")
                    .With(SystemMethods("MyNestedClass", "MyCompany.MyTechnology", TestClassName))
                    .With(Constructor(TestClassName)
                    .With(Nesting(TestClassName, "MyNestedClass")))
                  ).Colon()
            );
        }

        [TestMethod]
        public void AnonymousClass()
        {
            Assert.Inconclusive("Not verified");
        }
    }
}
