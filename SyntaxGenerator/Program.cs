using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"..\..\..\TaffyScript.Compiler\Syntax\Concrete";
            var nameSpace = "TaffyScript.Compiler.Syntax";
            // The following types have some functionality beyond state and can't just be generated:
            // * ImportObjectNode
            // * RootNode
            // * ConstantToken
            //
            GenerateSyntax(path, "TaffyScript.Compiler.Syntax", "Node", new List<string>()
            {
                "Root : List<UsingsNode> compilationUnits",
                "Assign : ISyntaxElement left, string op, ISyntaxElement right",
                "Locals : List<VariableDeclaration> locals",
                "Conditional : ISyntaxElement condition, ISyntaxElement left, ISyntaxElement right",
                "Logical : ISyntaxElement left, string op, ISyntaxElement right",
                "Bitwise : ISyntaxElement left, string op, ISyntaxElement right",
                "Equality : ISyntaxElement left, string op, ISyntaxElement right",
                "Relational : ISyntaxElement left, string op, ISyntaxElement right",
                "Shift : ISyntaxElement left, string op, ISyntaxElement right",
                "Additive: ISyntaxElement left, string op, ISyntaxElement right",
                "Multiplicative : ISyntaxElement left, string op, ISyntaxElement right",
                "Prefix : string op, ISyntaxElement right",
                "FunctionCall : ISyntaxElement callee, List<ISyntaxElement> arguments, TokenPosition endPosition",
                "MemberAccess : ISyntaxElement left, ISyntaxElement right",
                "ArrayAccess : ISyntaxElement left, List<ISyntaxElement> arguments",
                "Postfix : ISyntaxElement left, string op",
                "ArrayLiteral : List<ISyntaxElement> elements",
                "If : ISyntaxElement condition, ISyntaxElement thenBrach, ISyntaxElement elseBranch",
                "Switch : ISyntaxElement value, List<SwitchCase> cases, ISyntaxElement defaultCase",
                "While : ISyntaxElement condition, ISyntaxElement body",
                "Repeat : ISyntaxElement count, ISyntaxElement body",
                "With : ISyntaxElement target, ISyntaxElement body",
                "Do : ISyntaxElement body, ISyntaxElement condition",
                "For : ISyntaxElement initialize, ISyntaxElement condition, ISyntaxElement increment, ISyntaxElement body",
                "Return : ISyntaxElement result",
                "Block : List<ISyntaxElement> body",
                "ImportScript : string dotNetType, string methodName, string importName, List<string> arguments",
                "Enum : string name, List<EnumValue> values",
                "Script : string name, List<VariableDeclaration> arguments, ISyntaxElement body",
                "ArgumentAccess : ISyntaxElement index",
                "Object : string name, string parent, List<ScriptNode> scripts",
                "Usings : List<UsingDeclaration> usings, List<ISyntaxElement> declarations",
                "Namespace : string name, List<ISyntaxElement> declarations",
                "New : string typeName, List<ISyntaxElement> arguments, TokenPosition endPosition",
                "Lambda : string scope, List<VariableDeclaration> arguments, BlockNode body",
                "Base : List<ISyntaxElement> arguments, TokenPosition endPosition"
            });

            //Currently *Tokens cannot be generated.
            /*GenerateSyntax(path, nameSpace, "Token", new List<string>()
            {
                "Variable : string name",
                "End",
                "Break",
                "Continue",
                "ReadOnly : string name"
            });*/
        }

        private static void GenerateSyntax(string output, string nameSpace, string elementType, List<string> types)
        {
            foreach(var type in types)
            {
                var split = type.Split(':');
                var name = split[0].Trim();
                using(var writer = new StreamWriter(output + "/" + name + elementType + ".cs"))
                {
                    DefineType(writer, nameSpace, name, elementType, split.Length > 1 ? split[1].Trim() : "");
                }
            }
        }

        private static void DefineType(StreamWriter writer, string nameSpace, string syntaxType, string elementType, string fieldList)
        {
            var fields = fieldList.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            var typeName = syntaxType + elementType;
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine();
            writer.WriteLine("namespace " + nameSpace);
            writer.WriteLine("{");
            writer.WriteLine("    public class " + typeName + " : Syntax" + elementType);
            writer.WriteLine("    {");
            writer.WriteLine("        public override SyntaxType Type => SyntaxType." + syntaxType + ";");

            foreach (var field in fields)
            {
                var split = field.Split(' ');
                writer.WriteLine($"        public {split[0]} {char.ToUpper(split[1][0]) + split[1].Substring(1)} {{ get; }}");
            }
            writer.WriteLine();
            writer.Write($"        public {typeName}({fieldList}");
            if (fieldList.Length > 0)
                writer.Write(", ");
            writer.WriteLine("TokenPosition position)");
            writer.WriteLine("            : base(position)");
            writer.WriteLine("        {");

            foreach (var field in fields)
            {
                var name = field.Split(' ')[1];
                writer.WriteLine($"            {char.ToUpper(name[0]) + name.Substring(1)} = {name};");
            }

            writer.WriteLine("        }");
            writer.WriteLine();
            writer.WriteLine("        public override void Accept(ISyntaxElementVisitor visitor)");
            writer.WriteLine("        {");
            writer.WriteLine("            visitor.Visit(this);");
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");

        }
    }
}
