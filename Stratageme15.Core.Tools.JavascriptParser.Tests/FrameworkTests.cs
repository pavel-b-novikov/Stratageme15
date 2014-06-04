using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stratageme15.Core.Tools.JavascriptParser.Tests
{
    [TestClass]
    public class FrameworkTests : ParserTestBase
    {
        [TestMethod]
        public void TestFrameworks()
        {
            DirectoryInfo di = new DirectoryInfo(".\\Frameworks\\");
            var frameworks = di.GetFiles("*.js");
            foreach (var framework in frameworks)
            {
                Console.WriteLine("Parsering {0}",framework.Name);
                var program = ParseFile(framework.FullName);
            }
        }
    }
}
