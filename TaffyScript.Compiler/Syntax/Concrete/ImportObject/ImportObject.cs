using System;
using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class ImportObjectNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.ImportObject;
        public IConstantToken<string> ImportName { get; }
        public bool AutoImplement { get; set; }
        public ImportCasing Casing { get; private set; } = ImportCasing.Native;
        public bool WeaklyTyped { get; private set; } = true;
        public bool IncludeStandard { get; private set; } = false;
        public List<ImportObjectField> Fields { get; private set; } = new List<ImportObjectField>();
        public ImportObjectConstructor Constructor { get; set; }
        public List<ImportObjectMethod> Methods { get; private set; } = new List<ImportObjectMethod>();

        public ImportObjectNode(string value, TokenPosition position) 
            : base(value, position)
        {
            ImportName = new ConstantToken<string>(value, position, ConstantType.String, value);
        }

        public ImportObjectNode(IConstantToken<string> importName, string value, TokenPosition position)
            : base(value, position)
        {
            ImportName = importName;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void ParseArguments(IEnumerable<ObjectImportArgument> arguments, IErrorLogger logger)
        {
            var set = new HashSet<string>();
            foreach(var arg in arguments)
            {
                if (set.Contains(arg.Name))
                {
                    logger.Error("Tried to add the same impport argument multiple times", arg.Position);
                    continue;
                }

                switch(arg.Name)
                {
                    case "case":
                        switch(arg.Value)
                        {
                            case "pascal_case":
                                Casing = ImportCasing.Pascal;
                                break;
                            case "snake_case":
                                Casing = ImportCasing.Snake;
                                break;
                            case "camel_case":
                                Casing = ImportCasing.Camel;
                                break;
                            case "native_case":
                                Casing = ImportCasing.Native;
                                break;
                            default:
                                logger.Error("Invalid case option: " + arg.Value, arg.Position);
                                break;
                        }
                        break;
                    case "typing":
                        switch(arg.Value)
                        {
                            case "weak":
                                WeaklyTyped = true;
                                break;
                            case "strong":
                                WeaklyTyped = false;
                                break;
                            default:
                                logger.Error("Invalid typing option: " + arg.Value, arg.Position);
                                break;
                        }
                        break;
                    case "include_std":
                        if (bool.TryParse(arg.Value, out var include))
                            IncludeStandard = include;
                        else
                            logger.Error("Invalid include_std option: " + arg.Value, arg.Position);
                        break;
                    default:
                        logger.Error("Invalid import argument: " + arg.Name, arg.Position);
                        break;
                }
            }
        }
    }

    public struct ObjectImportArgument
    {
        public string Name;
        public string Value;
        public TokenPosition Position;

        public ObjectImportArgument(string name, string value, TokenPosition pos)
        {
            Name = name;
            Value = value;
            Position = pos;
        }
    }
}
