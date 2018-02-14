namespace TaffyScriptCompiler
{
    public interface ISymbol
    {
        SymbolType Type { get; }
        bool IsLeaf { get; }
        string Name { get; }
        SymbolNode Parent { get; }
    }
}
