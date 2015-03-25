using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Reactors.Basic.Tests.NodesComparing.SyntaxNodeComparers
{
    class VarLetModifierNodeComparer : SyntaxNodeComparerBase<VarLetModifier>
    {
        protected override bool NodesEquals(VarLetModifier actual, VarLetModifier expected)
        {
            return (actual.IsLet==expected.IsLet)&&(actual.IsVar==expected.IsVar);
        }
    }
}
