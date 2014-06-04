using System;

namespace Stratageme15.Core.JavascriptCodeDom.Exceptions
{
    public class UnexpectedException : Exception
    {
        public SyntaxTreeNodeBase TreeNode { get; private set; }

        public SyntaxTreeNodeBase This { get; private set; }

        public Enum Enum { get; private set; }

        public UnexpectedException(Enum @enum, SyntaxTreeNodeBase @this)
        {
            This = @this;
            Enum = @enum;
            if (TreeNode != null) _msg = string.Format("Unexpected {0} ({2}) in {1}", TreeNode.GetType().Name, This.GetType().Name, TreeNode.ToString());
            else _msg = string.Format("Unexpected operator {0} in {1}", Enum.GetType().Name, This.GetType().Name);
        }

        public UnexpectedException(SyntaxTreeNodeBase treeNode, SyntaxTreeNodeBase @this)
        {
            This = @this;
            TreeNode = treeNode;
            if (TreeNode != null) _msg = string.Format("Unexpected {0} ({2}) in {1}", TreeNode.GetType().Name, This.GetType().Name, TreeNode.ToString());
            else _msg = string.Format("Unexpected operator {0} in {1}", Enum.GetType().Name, This.GetType().Name);
        }

        private readonly string _msg;
        public override string Message
        {
            get { return _msg; }
        }
    }
}
