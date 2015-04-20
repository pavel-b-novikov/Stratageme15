using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stratageme15.Reactors.Basic.Tests.TranslationTests
{
    [TestClass]
    public class ExpressionsAndOperations : MethodBodyTestBase
    {
        
        [TestMethod]
        public void MathOperations()
        {
            AssertBodyTranslated(
                "int a = 1 + 2;",
                "var a = 1 + 2;",
                "+ operation"
                );
            AssertBodyTranslated(
                "int a = 1 * 2;",
                "var a = 1 * 2;",
                "* operation"
                );
            AssertBodyTranslated(
                "int a = 1 / 2;",
                "var a = 1 / 2;",
                "/ operation"
                );
            AssertBodyTranslated(
                "int a = 1 - 2;",
                "var a = 1 - 2;",
                "- operation"
                );
            AssertBodyTranslated(
                "int a = (1 + 2)*2;",
                "var a = (1 + 2)*2;",
                "() operation"
                );
            AssertBodyTranslated(
                "int a = (1 + 2)*(2 + 3);",
                "var a = (1 + 2)*(2 + 3);",
                "() operation"
                );
        }

        [TestMethod]
        public void LogicalOperations()
        {
            Assert.Inconclusive("Not verified");
        }

        [TestMethod]
        public void UnaryExpressions()
        {
            AssertBodyTranslated(
                "int a = -1;",
                "var a = -1;",
                "unary - operation"
                );
            AssertBodyTranslated(
                "int a = -1 + 2;",
                "var a = -1 + 2;",
                "unary () - operation"
                );
        }

        [TestMethod]
        public void ThisExpression()
        {
            // todo CallExpression/CallStatement conflict
            AssertBodyTranslated(
                "this.ToString();",
                "this.ToString();",
                "this call operation"
                );
        }

        [TestMethod]
        public void ParenthesizedExpressions()
        {
            Assert.Inconclusive("Not verified");
        }

        [TestMethod]
        public void MemberAccess()
        {
            Assert.Inconclusive("Not verified");
        }

        [TestMethod]
        public void ConditionalExpression()
        {
            Assert.Inconclusive("Not verified");
        }

        [TestMethod]
        public void SimpleArrayCreation()
        {
            Assert.Inconclusive("Not verified");
        }

        [TestMethod]
        public void ExplicitArrayCreation()
        {
            Assert.Inconclusive("Not verified");
        }

        [TestMethod]
        public void ArrayAccess()
        {
            Assert.Inconclusive("Not verified");
        }
    }
}
