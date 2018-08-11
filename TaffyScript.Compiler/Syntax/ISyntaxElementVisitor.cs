using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.Syntax
{
    public interface ISyntaxElementVisitor
    {
        void Visit(AdditiveNode additive);
        void Visit(ArgumentAccessNode argumentAccess);
        void Visit(ArrayAccessNode arrayAccess);
        void Visit(ArrayLiteralNode arrayLiteral);
        void Visit(AssignNode assign);
        void Visit(BaseNode baseNode);
        void Visit(BitwiseNode bitwise);
        void Visit(BlockNode block);
        void Visit(BreakToken breakToken);
        void Visit(ConditionalNode conditional);
        void Visit(IConstantToken constantToken);
        void Visit(ContinueToken continueToken);
        void Visit(DoNode @do);
        void Visit(EndToken endToken);
        void Visit(EnumNode enumDeclaration);
        void Visit(EqualityNode equality);
        void Visit(ForNode @for);
        void Visit(FunctionCallNode functionCall);
        void Visit(IfNode @if);
        void Visit(ImportObjectNode importObjectNode);
        void Visit(ImportScriptNode import);
        void Visit(LambdaNode lambdaNode);
        void Visit(LocalsNode locals);
        void Visit(LogicalNode logical);
        void Visit(MemberAccessNode memberAccess);
        void Visit(MultiplicativeNode multiplicative);
        void Visit(NamespaceNode namespaceNode);
        void Visit(NewNode newNode);
        void Visit(ObjectNode objectNode);
        void Visit(PostfixNode postfix);
        void Visit(PrefixNode prefix);
        void Visit(ReadOnlyToken readOnlyToken);
        void Visit(RelationalNode relational);
        void Visit(RepeatNode repeatNode);
        void Visit(ReturnNode @return);
        void Visit(RootNode root);
        void Visit(ScriptNode script);
        void Visit(ShiftNode shift);
        void Visit(SwitchNode @switch);
        void Visit(UsingsNode usingsNode);
        void Visit(VariableToken variableToken);
        void Visit(WhileNode @while);
    }
}
