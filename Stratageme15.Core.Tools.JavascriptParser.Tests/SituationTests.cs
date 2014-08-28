using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Statements;
#pragma warning disable 168
namespace Stratageme15.Core.Tools.JavascriptParser.Tests
{
    [TestClass]
    public class SituationTests : ParserTestBase
    {
        [TestMethod]
        public void ExpressionAsOperator()
        {
            const string testCase =
@"function() {
    val == undefined 
}";


           var program = Parse(testCase);
        }

        [TestMethod]
        public void ComplexTernaryStatement()
        {
            const string testCase =
@"function() {
    val == undefined 
            ? support.attributes || !documentIsHTML 
                    ? elem.getAttribute(name) 
                    : (val = elem.getAttributeNode(name)) && val.specified 
                        ? val.value 
                        : null 
            : val;    
}";

            var program = Parse(testCase);
        }

        [TestMethod]
        public void MultipleAssignmentWithRegexps()
        {
            string[] regex = new[]
                                       {
                                           @"/d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|""[^""]*""|'[^']*'/g",
                                           @"/\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g"
                                           ,
                                           @"/[^-+\dA-Z]/g"
                                       };
            string testCase =

@"
var token = "+ regex[0]+ @",
		timezone = " + regex[1] +@",
		timezoneClip = "+ regex[2] + @",
		pad = function (val, len) {
		    val = String(val);
		    len = len || 2;
		    while (val.length < len) val = ""0"" + val;
		    return val;
		};
";
            var program = Parse(testCase);
            Assert.AreEqual(1, program.CodeElements.Count);
            Assert.AreSame(typeof(VariableDefStatement),program.CodeElements[0].GetType());
            var vds = (VariableDefStatement) program.CodeElements[0];
            Assert.AreEqual(4,vds.Variables.Count);
            for (int i = 0; i < 3; i++)
            {
                var v = vds.Variables[i];
                Assert.AreSame(typeof(RegexLiteral),v.Item3.GetType());
                var lt = (RegexLiteral) v.Item3;
                Assert.AreEqual(regex[i],lt.RegexString);
            }
        }

        [TestMethod]
        public void OnlyParenthesisSequenceBug()
        {
            const string testCase =
                @"var a = !wrapMap[(rtagName.exec(value) || ["""", """"])[1].toLowerCase()];";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void StrangeRegex()
        {
            const string testCase =
                @"var rheaders = /^(.*?):[ \t]*([^\r\n]*)$/mg;";
            var program = Parse(testCase);
            
        }

        [TestMethod]
        public void RegexTestTernary()
        {
            const string testCase =
                @"var t = r.test("" "") ? /^[\s\xA0]+|[\s\xA0]+$/g : /^\s+|\s+$/g, b = c;";
            var program = Parse(testCase);

        }
        
        [TestMethod]
        public void RegexWhitespaceSensetivity()
        {
            const string testCase =
                @"jQuery.expr.match.bool.source.match( /\w+/g );";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void EmptyForIn()
        {
            const string testCase =
                @"for (d in a);";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void RegexObject()
        {
            const string testCase =
@"
    var o = { 
        contents: 
            { xml: /xml/}
    }
";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void NewlineExpression()
        {
            const string testCase =
@"
F -= (F - (H % F || F)) / u (H /
                        F)
";
            var program = Parse(testCase); // modified regex first char without whitespace
        }

        [TestMethod]
        public void DivBuggyWhitespace()
        {
            const string testCase =
@"
F -= (F - (H % F || F)) /u (H /
                        F)
";
            var program = Parse(testCase); 
        }
        
        [TestMethod]
        public void ComplexExpressionStatement()
        {
            const string testCase =
@"
p.fx.timer = function (a) {
    a() && p.timers.push(a) && !cN && (cN = setInterval(p.fx.tick, p.fx.interval));
}
";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void KeywordObjectKeys()
        {
            const string testCase =
@"
var o = {
     this: a,
     break: b,
     in: c,
     of: c,
};";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void UseStrictDirective()
        {
            const string testCase =
@"
function (factory) {
    'use strict';
    if (typeof define === 'function' && define.amd) {
        define(['jquery'], factory);
    } else {
        factory(window.jQuery);
    }
}
";
            var program = Parse(testCase);
            var f = program.CodeElements[0] as FunctionDefExpression;
            Assert.IsTrue(f.Code.Statements.First.Value is UseStrict);
        }

        [TestMethod]
        public void EmptyWhile()
        {
            const string testCase =
@"
while (e[n++] = t[r++]);
";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void CodeBlock()
        {
            const string testCase =
@"
var t, n, r, i = this[0]; {
    if (arguments.length) return r = x.isFunction(e), this.each(function (n) {
        var i;
        1 === this.nodeType && (i = r ? e.call(this, n, x(this).val()) : e, null == i ? i = """" : ""number"" == typeof i ? i += """" : x.isArray(i) && (i = x.map(i, function (e) {
            return null == e ? """" : e + """"
        })), t = x.valHooks[this.type] || x.valHooks[this.nodeName.toLowerCase()], t && ""set"" in t && t.set(this, i, ""value"") !== undefined || (this.value = i))
    });
    if (i) return t = x.valHooks[i.type] || x.valHooks[i.nodeName.toLowerCase()], t && ""get"" in t && (n = t.get(i, ""value"")) !== undefined ? n : (n = i.value, ""string"" == typeof n ? n.replace($, """") : null == n ? """" : n)
}
";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void MemberAccess()
        {
            const string testCase =
@"
var t= x.valHooks[this.nodeName.toLowerCase()];
";
            var program = Parse(testCase);
        }


        [TestMethod]
        public void StrangeOnelineFunction()
        {
            const string testCase =
                @"
function a(){ return v.replace(""\\"") } function b() {return 1;};
";
            var program = Parse(testCase);


        }

        [TestMethod]
        public void DoubleNumberClosing()
        {
           
                const string testCase =
                    @"
function() {
    var s1 = 2.toString();
}
";
            var program = Parse(testCase);

            var fStatements = ((FunctionDefExpression) program.CodeElements[0]).Code.Statements;
            Assert.IsTrue(fStatements.First.Value.GetType()==typeof(VariableDefStatement));
            Assert.IsTrue(fStatements.First.Next.Value.GetType() == typeof(CallStatement));
            
            const string testCase2 =
                   @"
function() {
    var s1 = 2..toString();
}
";
            var program2 = Parse(testCase2);
            var fStatements2 = ((FunctionDefExpression)program2.CodeElements[0]).Code.Statements;

        }


        [TestMethod]
        public void Debugger()
        {
            const string testCase =
@"
function(){
  if (a>b) { debugger }
   else {debugger }
}
";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void OneLineExpressionReadsAsRegex()
        {
            const string testCase =
@"
a = function () { var r = n / f.duration || 0, i = 1 - r, s = 0; return 0 }, b = function () { var i = /[\-+]?(?:\d*\.|)\d+(?:[eE][\-+]?\d+|)/.source;}
";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void WeirdArrayCreation()
        {
            const string testCase =
@"
var i = (parseInt($(ce).css(""borderLeftWidth""), 10) || 0);
var a = [(parseInt($(ce), 10) || 0) + (parseInt($(ce), 10) || 0)];
";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void StatementLabels()
        {
            const string testCase =
@"
function(t, event) {

    var m = 1;
    var type = event ? event.type : null; // workaround for #2317
    var list = null;

    droppablesLoop: for (var i = 0; i < m.length; i++) {
        for (var j=0; j < list.length; j++) { if(true) { continue droppablesLoop; } }; //Filter out elements in the current dragged item
        m[i].visible = m[i].element.css(""display"") != ""none""; 
        if(!m[i].visible) continue; 
    }

}
";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void EmptyStatementTest()
        {
            const string testCase =
@"
;(jQuery.effects || (function($, undefined) { } ) )
";
            var program = Parse(testCase);
            Assert.IsTrue(program.CodeElements[0] is EmptyStatement);
        }

        //isFixed |= $(this).css('position') == 'fixed';

        [TestMethod]
        public void OrAssignTest()
        {
            const string testCase =
@"
isFixed |= $(this).css('position') == 'fixed';
";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void KeywordObjectFieldsAgain()
        {
            const string testCase =
@"
if ( !options || !options.of ) {
		return _position.apply( this, arguments );
	}
";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void ElseIfs()
        {
            const string testCase =
@"
if (/^(se|s|e)$/.test(a)) {
    that.size.width = os.width + ox;
    that.size.height = os.height + oy;
}
else if (/^(ne)$/.test(a)) {
    that.size.width = os.width + ox;
    that.size.height = os.height + oy;
    that.position.top = op.top - oy;
}
else if (/^(sw)$/.test(a)) {
    that.size.width = os.width + ox;
    that.size.height = os.height + oy;
    that.position.left = op.left - ox;
}
else {
    that.size.width = os.width + ox;
    that.size.height = os.height + oy;
    that.position.top = op.top - oy;
    that.position.left = op.left - ox;
}
";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void NestedIfs()
        {
            const string testCase =
@"
if (literal)
    if (format.charAt(iFormat) == ""'"" && !lookAhead(""'""))
        literal = false;
    else
        checkLiteral();
else
    switch (format.charAt(iFormat)) {
        case 'd':
            day = getNumber('d');
            break;
        default:
            checkLiteral();
    }
";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void DanglingStrings()
        {
            const string testCase =
@"
var n=""#""+e.id.replace(/\\\\/g,""\\"");e.dpDiv.find(""[data-handler]"");
";
            var program = Parse(testCase);
        }

       
        [TestMethod]
        public void HexAndUnicodeLiterals()
        {
            const string testCase =
@"
var s = ""\uFEFF\xA0"";
";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void InvalidString()
        {
            const string testCase =
@"
var s = '//cdnjs.cloudflare.com/ajax/libs/jquery-mousewheel/3.0.6/jquery.mousewheel.min.js""><\/script>';
";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void RegexWithSlash()
        {
            const string testCase =
@"
function f(a){return a.replace(/([!""#$%&'()*+,./:;<=>?@\[\\\]^`{|}~])/g,""\\$1"")}
";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void OneMoreInvalidString()
        {
            const string testCase =
@"
var s =""<!--[if gt IE ""+ ++ba+""]><i></i><![endif]--\>"";;
";
            var program = Parse(testCase);
        }

        [TestMethod]
        public void StrangeIfCondition()
        {
            const string testCase =
@"
if (r) {
        if (A) return a, i
    } else if B) return a, i
";
            var program = Parse(testCase);
        }
    }
}
