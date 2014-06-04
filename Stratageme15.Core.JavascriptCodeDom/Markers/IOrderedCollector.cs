namespace Stratageme15.Core.JavascriptCodeDom.Markers
{
    /// <summary>
    /// Interface providing sunctionality not only for sequential collectind child nodes, but allowing to reorder them dusing collection
    /// </summary>
    public interface IOrderedCollector
    {
        /// <summary>
        /// Places collected symbol at he beginning of the code
        /// </summary>
        /// <param name="symbol">Syntax tree node</param>
        void CollectSymbolAtStart(SyntaxTreeNodeBase symbol);
    }
}
