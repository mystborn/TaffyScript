namespace TaffyScriptCompiler.Backend
{
    public interface ICompiler
    {
        CompilerResult CompileProject(string projectDir);
        CompilerResult CompileCode(string code, BuildConfig config);
        CompilerResult CompileCode(string code, string output);
    }
}
