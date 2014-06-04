using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Literals;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Tools.JavascriptParser.Tests
{
    [TestClass]
    public class ParserTestBase
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        protected string ParseringDebug { get { return _parseringDebugSb.ToString(); } }
        private static FileStream _fs = null;
        private static StringBuilder _parseringDebugSb;
        private static TextWriter _parseringDebugSw;

        public ParserTestBase()
        {
            _parseringDebugSb = new StringBuilder();
        }
        protected JsProgram ParseFile(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            using (var logFs = new FileStream(string.Format(@"J:\TestLogs\parseDebug_{0}.txt", fi.Name), FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (TextWriter tw = new StreamWriter(logFs))
                {
                    NodeInfoTree._debugWriter = tw;
                    using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        return ParseCore(fs,fi.Name);
                    }

                }

            }
        }

        protected JsProgram Parse(string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return ParseCore(ms);
            }

        }

        private JsProgram ParseCore(Stream s,string fileName=null)
        {
            if (string.IsNullOrEmpty(fileName)) fileName = TestContext.TestName;
            var scanner = new Scanner(s);
            var parser = new Parser(scanner);
            try
            {
                return parser.Parse();
            }
            catch (ParseringException e)
            {
               
                _parseringDebugSw.Flush();
                using (var fse = new FileStream(string.Format(@"J:\TestLogs\ERROR_{0}.txt", fileName), FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (TextWriter tw = new StreamWriter(fse))
                    {
                        NodeInfoTree._debugWriter = tw;
                        e.TreePart.DebugPrint();
                        tw.WriteLine(e.Message);
                        tw.Flush();
                    }
                }
                TestContext.WriteLine(e.ToString());
                string se = ParseringDebug;
                throw;
            }
        }

        [TestInitialize]
        public void Init()
        {
            _parseringDebugSb.Clear();
            _fs = new FileStream(string.Format(@"J:\TestLogs\parseDebug_{0}.txt", TestContext.TestName), FileMode.OpenOrCreate, FileAccess.Write);

            _parseringDebugSw = new StreamWriter(_fs);
            NodeInfoTree._debugWriter = _parseringDebugSw;
        }

        protected void TestRegex(string regex)
        {
            var rxVar = string.Format("var a = {0};", regex);
            var program = Parse(rxVar);
            Assert.AreEqual(1, program.CodeElements.Count);
            VariableDefStatement vds = program.CodeElements[0] as VariableDefStatement;
            Assert.IsNotNull(vds);
            Assert.AreEqual(1, vds.Variables.Count);
            var v = vds.Variables[0];
            var rx = v.Item3 as RegexLiteral;
            Assert.IsNotNull(rx);
            Assert.AreEqual(regex, rx.RegexString);
        }
    }
}
