using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Logging;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.Repositories;

namespace Stratageme15.Reactors.Basic.Tests
{
    public class BasicBatchTestBase : StratagemeTestBase
    {
        protected const string TestClassName = "MyClass";

        [TestInitialize]
        public new void Init()
        {
            base.Init();
        }

        public string CurrentCode { get; private set; }

        private JsProgram Translate(string code)
        {
            return Translate(code, new BasicReactorBatch());
        }

        public Translator InitializeTranslator(string code, out SyntaxTree synTree)
        {
            return InitializeTranslatorWithContext(code, out synTree, new BasicReactorBatch());
        }

        protected void AssertTranslated(string csCodeSource, string jsCodeExpected, string message = null)
        {
            var expected = Parse(jsCodeExpected);
            var actual = Translate(csCodeSource);
            CurrentCode = Code(actual);
            actual = Parse(CurrentCode);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("EXPECTED:");
            sb.AppendLine(jsCodeExpected);
            sb.AppendLine("ACTUAL:");
            sb.AppendLine(CurrentCode);
            Assert.IsTrue(Compare(actual, expected), string.Format("{0}\n{1}", message, sb.ToString()));
        }

    }
}
