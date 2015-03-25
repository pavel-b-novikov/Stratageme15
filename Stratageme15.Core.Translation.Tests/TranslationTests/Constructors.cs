using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stratageme15.Reactors.Basic.Tests.TranslationTests
{
    [TestClass]
    public class Constructors : BasicBatchTestBase
    {
        [TestMethod]
        public void SimpleConstructor()
        {
            AssertTranslated(
             @"
            public class MyClass {
                public Myclass(){

                }
            }
            ",
             @"
            var MyClass = (function(){
               function MyClass() {

               }
               return MyClass;
            })();
            ;
            ");
        }


        [TestMethod]
        public void StaticConstructor()
        {
           Assert.Inconclusive("Not verified");
        }
    }
}
