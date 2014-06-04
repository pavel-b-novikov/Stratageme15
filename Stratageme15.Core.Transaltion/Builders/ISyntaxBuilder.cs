using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratageme15.Core.JavascriptCodeDom;

namespace Stratageme15.Core.Transaltion.Builders
{
    interface ISyntaxBuilder<TResult> where TResult : SyntaxTreeNodeBase
    {
        TResult Build();
    }
}
