using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Syntax
{
    public interface ISyntaxElementVisitor
    {
        void Visit(RootNode root);
        void Visit(AssignNode assign);
        void Visit(ReadOnlyToken readOnlyToken);
        void Visit(ExitToken exitToken);
        void Visit(ObjectNode objectNode);
        void Visit(EventNode eventNode);
        void Visit(ContinueToken continueToken);
        void Visit(BreakToken breakToken);
        void Visit(RepeatNode repeatNode);
        void Visit(EndToken endToken);
        void Visit(VariableToken variableToken);
        void Visit(CaseNode @case);
        void Visit(BitwiseNode bitwise);
        void Visit(LocalsNode locals);
        void Visit(ScriptNode script);
        void Visit(ArgumentAccessNode argumentAccess);
        void Visit(BlockNode block);
        void Visit(EnumNode enumDeclaration);
        void Visit(ReturnNode @return);
        void Visit(IConstantToken constantToken);
        void Visit(ForNode @for);
        void Visit(DoNode @do);
        void Visit(WithNode with);
        void Visit(WhileNode @while);
        void Visit(DefaultNode @default);
        void Visit(PrefixNode prefix);
        void Visit(SwitchNode @switch);
        void Visit(IfNode @if);
        void Visit(DeclareNode declare);
        void Visit(ArrayLiteralNode arrayLiteral);
        void Visit(ImportNode import);
        void Visit(PostfixNode postfix);
        void Visit(ArrayAccessNode arrayAccess);
        void Visit(ExplicitArrayAccessNode explicitArrayAccess);
        void Visit(MapAccessNode mapAccess);
        void Visit(GridAccessNode gridAccess);
        void Visit(ListAccessNode listAccess);
        void Visit(MemberAccessNode memberAccess);
        void Visit(FunctionCallNode functionCall);
        void Visit(ConditionalNode conditional);
        void Visit(MultiplicativeNode multiplicative);
        void Visit(AdditiveNode additive);
        void Visit(ShiftNode shift);
        void Visit(RelationalNode relational);
        void Visit(EqualityNode equality);
        void Visit(LogicalNode logical);
    }
}
