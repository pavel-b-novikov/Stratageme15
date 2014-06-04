using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Binary;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Tools.JavascriptParser.Tests
{
    [TestClass]
    public class BasicGrammarTests : ParserTestBase
    {
        [TestMethod]
        public void EmptyProgramTest()
        {
            try
            {
                var result = Parse(string.Empty);
            }catch(ParseringException pe)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void DoWhileStatement()
        {
            const string testCase =
@"function() {
    do {
                    
    } while ((elem = elem.parentNode) && elem.nodeType === 1);
}";
            var program = Parse(testCase);
            
            FunctionDefExpression fun = program.CodeElements[0] as FunctionDefExpression;
            Assert.IsNotNull(fun);
            var doWhile = fun.Code.Statements.First.Value as DoWhileStatement;
            Assert.IsNotNull(doWhile);
            Assert.AreEqual(typeof(ParenthesisExpression),doWhile.WhileExpression.GetType());
        }

        [TestMethod]
        public void IfStatement()
        {
            const string testCase =
@"function() {
    if (a) a();
    else if (c) d();
    else e();
}";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void BasicRegexes()
        {
            TestRegex(@"/d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|""[^""]*""|'[^']*'/g");
            TestRegex(@"/\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g");
            TestRegex(@"/[^-+\dA-Z]/g");
            TestRegex(@"/\?/");
            TestRegex(@"/#.*$/");
            TestRegex(@"/([?&])_=[^&]*/");
            TestRegex(@"/\w+/g ");
            TestRegex(@"/[\-+]?(?:\d*\.|)\d+(?:[eE][\-+]?\d+|)/");
            
        }

        [TestMethod]
        public void StrangeRegex1()
        {
            TestRegex(@"/^(.*?):[ \t]*([^\r\n]*)$/mg");
        }

        [TestMethod]
        public void ForOfWithPreviouslyVarDeclarated()
        {
            const string testCase =
                @"for (d of a) { d() };";
            var program = Parse(testCase);
            Assert.AreEqual(1,program.CodeElements.Count);
            var forof = program.CodeElements[0] as ForOfStatement;
            Assert.IsNotNull(forof);
        }

        [TestMethod]
        public void ForInForOf()
        {
            const string testCase1 =
                @"for (var d of a) { d() };";
            var program = Parse(testCase1);
            const string testCase2 =
                @"for (let d of a) { d() };";
            program = Parse(testCase2);
            const string testCase3 =
                @"for (var d in a) { d() };";
            program = Parse(testCase1);
            const string testCase4 =
                @"for (let d in a) { d() };";
            program = Parse(testCase2);
        }

        [TestMethod]
        public void SwitchStatement()
        {
            const string testCase =
@"
    switch (a) {
        case 'only':
        case 'first':
            while (c = c.previousSibling)
                if (c.nodeType === 1) return !1;
            if (a === 'first') return !0;
            c = b;
        case 'last':
            while (c = c.nextSibling)
                if (c.nodeType === 1) return !1;
            return !0
    }
";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void UndefinedIsNotAKeyword()
        {
            const string testCase =
                @"
function f(window, undefined) { }
";
            var program = Parse(testCase);
        }
        [TestMethod]
        public void Numbers()
        {
            const string testCase =
                @"
function f() {
    var a = 1;
    var b = 1.1;
    var c = 1.;
    var d = 1. + .5;
    var e = 1e-5;
    var e2 = 1E+5;
}
";
            var program = Parse(testCase);
        }
    }
}
