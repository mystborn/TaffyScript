namespace TaffyScript.Compiler.Syntax
{
    public interface ISyntaxToken : ISyntaxElement
    {
        string Name { get; }
    }
}
