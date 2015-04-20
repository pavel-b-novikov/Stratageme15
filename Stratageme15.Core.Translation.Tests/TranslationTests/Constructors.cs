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
                public MyClass(){

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
            AssertTranslated(
              @"
            public class MyClass {
                public MyClass(){
                    static MyClass(){
                        int i = 0;
                    }
                }
            }
            ",
              @"
            var MyClass = (function(){
               function MyClass() {

               }

                (function(_cls){ 
                    var i = 0;
                })(MyClass);

               return MyClass;
            })();
            ;
            ");
        }

        
    }
}
