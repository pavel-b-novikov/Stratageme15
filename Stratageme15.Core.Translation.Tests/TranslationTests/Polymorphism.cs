using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stratageme15.Reactors.Basic.Tests.TranslationTests
{
    [TestClass]
    public class Polymorphism : BasicBatchTestBase
    {
        [TestMethod]
        public void SimplePolymorphism()
        {
            AssertTranslated(
    @"
            public class MyClass {
                public int DoSomething(){
                    return 0;
                }
                public string DoSomething(string str){
                    return str.ToLower();
                }
            }
            ", J.Class(TestClassName, J.Constructor(TestClassName)
             .With(J.Method(TestClassName, "DoSomething",

             body:
@"
	    if (__matchArguments(arguments)){
		    return 0;
	    } else if (__matchArguments(arguments,""string"")){
		    var str = arguments[0];
				
		    return str.ToLower();
	    }
"
             ))
                ).Colon()
            );
        }

        [TestMethod]
        public void ConstructorPolymorphism()
        {
            Assert.Inconclusive("Not verified");
        }

        [TestMethod]
        public void StaticPolymorphism()
        {
            Assert.Inconclusive("Not verified");
        }

        [TestMethod]
        public void PrivatePolymorphism()
        {
            Assert.Inconclusive("Not verified");
        }

        [TestMethod]
        public void MixOfStaticAndPrivatePolymorphism()
        {
            Assert.Inconclusive("Not verified");
        }

        [TestMethod]
        public void ParamsPolymorphism()
        {
            Assert.Inconclusive("Not verified");
        }
       
    }
}
