namespace TaffyScript.Compiler
{
    public interface ISymbol
    {
        SymbolType Type { get; }
        SymbolScope Scope { get; }
        string Name { get; }
        bool IsLeaf { get; }
        SymbolNode Parent { get; }
    }
}
