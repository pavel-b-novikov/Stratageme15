using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratageme15.Core.JavascriptCodeDom.Exceptions
{
    public class RoleChangedException : Exception
    {
        public SyntaxTreeNodeBase Node {get; private set; }
        public string OldRole { get; private set; }
        public string NewRole { get; private set; }

        public RoleChangedException(SyntaxTreeNodeBase node, string oldRole, string newRole)
        {
            Node = node;
            OldRole = oldRole;
            NewRole = newRole;
        }

        public override string Message
        {
            get
            {
                return
                    string.Format(
                        "Tried to change role of node {0} from {1} to {2}. Node roles could be set only once", Node,
                        OldRole, NewRole);
            }
        }
    }
}
