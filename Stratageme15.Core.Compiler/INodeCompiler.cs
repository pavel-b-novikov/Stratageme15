using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratageme15.Core.JavascriptCodeDom;

namespace Stratageme15.Core.Compiler
{
    public interface INodeCompiler
    {
        NodeCompilersRepository Repository { get; set; }
        void Compile(TextWriter output, SyntaxTreeNodeBase node);
    }
}
