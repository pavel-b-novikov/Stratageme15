using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stratageme15.Core.Compiler;
using Stratageme15.Core.JavascriptCodeDom;

namespace Stratageme15.Reactors.Basic.Tests.TranslationTests
{
    [TestClass]
    public class ClassDeclaration : MethodBodyTestBase
    {
        [TestMethod]
        public void SimpleClassDeclaration()
        {
            AssertTranslated(
                @"
            public class MyClass {

            }
            ", J.Class(TestClassName, J.Constructor(TestClassName)).Colon()
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
            ", J.NsClass(TestClassName, "MyCompany", J.Constructor(TestClassName)).Colon()
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
            ", J.NsClass(TestClassName, "MyCompany.MyTechnology", J.Constructor(TestClassName)).Colon()
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
            ", J.NsClass(TestClassName, "MyCompany.MyTechnology", J.Constructor(TestClassName)).Colon()
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
            ", J.Class(TestClassName, J.Constructor(TestClassName)
                    .With(J.Constructor("MyNestedClass")       
                    .With(J.SystemMethods("MyNestedClass", null, TestClassName))
                    .With(J.Nesting(TestClassName, "MyNestedClass")))
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
            ", J.NsClass(TestClassName, "MyCompany",
             J.Constructor(TestClassName)
                    .With(J.Constructor("MyNestedClass"))
                    .With(J.SystemMethods("MyNestedClass", "MyCompany", TestClassName))
                    .With(J.Nesting(TestClassName, "MyNestedClass"))
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
            ", J.NsClass(TestClassName, "MyCompany.MyTechnology",
                    J.Constructor(TestClassName)
                    .With(J.Constructor("MyNestedClass"))
                    .With(J.SystemMethods("MyNestedClass", "MyCompany.MyTechnology", TestClassName))
                    .With(J.Nesting(TestClassName, "MyNestedClass"))
                  ).Colon()
            );
        }

        [TestMethod]
        public void AnonymousClass()
        {
            AssertBodyTranslated(
                @"var a = new { FirstName = ""Vasiliy"",Age = 15, IsMarried = false}",
                @"var a = { FirstName : ""Vasiliy"", Age : 15, IsMarried : false}",
                "anonymous object creation"
                );
        }
    }
}
