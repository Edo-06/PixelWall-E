public interface IVisitor<T>
{
    List<Exception> errors {get; set;}
    T Visit(ProgramNode program);

    T Visit(UnaryOpNode unaryOp);
    T Visit(BinaryOpNode binaryOp);
    T Visit(LiteralNode literal);
    T Visit(FunctionNode function);
    T Visit(VariableNode variable);

    T Visit(AssignmentNode assignment);
    T Visit(CommandNode command);
    T Visit(GoToNode goTo);
    T Visit(LabelNode label);
}