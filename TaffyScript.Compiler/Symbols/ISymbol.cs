namespace TaffyScript.Compiler
{
    public interface ISymbol
    {
        SymbolType Type { get; }
        SymbolScope Scope { get; }
        bool IsLeaf { get; }
        string Name { get; }
        SymbolNode Parent { get; }
    }
}
