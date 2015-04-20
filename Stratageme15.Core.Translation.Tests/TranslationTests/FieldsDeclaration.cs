using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stratageme15.Reactors.Basic.Tests.TranslationTests
{
    [TestClass]
    public class FieldsDeclaration : BasicBatchTestBase
    {
        [TestMethod]
        public void NumberField()
        {
            AssertTranslated(
 @"
            public class MyClass {
                public int Integer;             
                private float PrivateInteger;
                public short InitInteger = 25;
                private decimal PrivateInitInteger = 25;

                public MyClass(){
                    
                }
            }
            ",
 @"
            var MyClass = (function(){
                function MyClass() {
                    this.Integer = 0;
                    this.PrivateInteger = 0;
                    this.InitInteger = 25;
                    this.PrivateInitInteger = 25;
                }
                return MyClass;
            })();
            ;
            ");
        }

        [TestMethod]
        public void BooleanField()
        {
            AssertTranslated(
 @"
            public class MyClass {
                public bool BoolField;             
                private bool PrivateBoolField;
                public bool InitBoolField = true;
                private bool PrivateInitBoolField = false;

                public MyClass(){
                    
                }
            }
            ",
 @"
            var MyClass = (function(){
                function MyClass() {
                    this.BoolField = false;
                    this.PrivateBoolField = false;
                    this.InitBoolField = true;
                    this.PrivateInitBoolField = false;
                }
                return MyClass;
            })();
            ;
            ");
        }

        [TestMethod]
        public void StringField()
        {
            AssertTranslated(
 @"
            public class MyClass {
                public string StringField;             
                private string PrivateStringField;
                public string InitStringField = ""abc"";
                private string PrivateInitStringField = ""cba"";

                public MyClass(){
                    
                }
            }
            ",
 @"
            var MyClass = (function(){
                function MyClass() {
                    this.StringField = null;
                    this.PrivateStringField = null;
                    this.InitStringField = ""abc"";
                    this.PrivateInitStringField = ""cba"";
                }
                return MyClass;
            })();
            ;
            ");
        }

        [TestMethod]
        public void ReferenceTypeField()
        {
            AssertTranslated(
 @"
            using System;
            public class MyClass {
                public Console ConsoleField;             
                private int[] ArrayField;
                public object ObjectField = new object();
                private DateTime DateTimeField = new DateTime();

                public MyClass(){
                    
                }
            }
            ",
 @"
            var MyClass = (function(){
                function MyClass() {
                    this.ConsoleField = null;
                    this.ArrayField = null;
                    this.ObjectField = new object();
                    this.DateTimeField = new DateTime();
                }
                return MyClass;
            })();
            ;
            ");
        }

        [TestMethod]
        public void InitializedField()
        {
            AssertTranslated(
 @"
            public class MyClass {
                public string StaticString = ""abc"";
                public int Integer = 0;
                public Console RefType = new Console();

                private string PrivateStaticString = ""abc"";
                private int PrivateInteger = 0;
                private Console PrivateRefType = new Console();

                public MyClass(){
                    
                }
            }
            ",
 @"
            var MyClass = (function(){

                function MyClass() {
                    this.StaticString = ""abc"";
                    this.Integer = 0;
                    this.RefType = new Console();

                    this.PrivateStaticString = ""abc"";
                    this.PrivateInteger = 0;
                    this.PrivateRefType = new Console();
                }

                return MyClass;
            })();
            ;
            ");
        }

        [TestMethod]
        public void StaticField()
        {
            AssertTranslated(
             @"
            public class MyClass {
                public static string StaticString;
                public static int NonInitInteger;
                
                private static string PrivateStaticString;
                private static int PrivateNonInitInteger;

                public MyClass(){
                    static MyClass(){
                        NonInitInteger = 5;
                        PrivateNonInitInteger = 10;
                    }
                }
            }
            ",
             @"
            var MyClass = (function(){

                PrivateStaticString = null;
                PrivateNonInitInteger = 0;

                function MyClass() {

                }

                MyClass.StaticString = null;
                MyClass.NonInitInteger = 0;            

                (function(_cls){ 

                    MyClass.NonInitInteger = 5;
                    PrivateNonInitInteger = 10;

                })(MyClass);

                return MyClass;
            })();
            ;
            ");
        }

        [TestMethod]
        public void StaticInitializedField()
        {
            AssertTranslated(
             @"
            public class MyClass {
                public static int Integer = 0;
                public static Console RefType = new Console();

                private static int PrivateInteger = 0;
                private static Console PrivateRefType = new Console();

                public MyClass(){
                    static MyClass(){
                        NonInitInteger = 5;
                        PrivateNonInitInteger = 10;
                    }
                }
            }
            ",
             @"
            var MyClass = (function(){
                PrivateInteger = 0;
                PrivateRefType = new Console();

                function MyClass() {

                }

                MyClass.Integer = 0;
                MyClass.RefType = new Console();                

                (function(_cls){ 

                    MyClass.NonInitInteger = 5;
                    PrivateNonInitInteger = 10;

                })(MyClass);

                return MyClass;
            })();
            ;
            ");
        }


    }
}
