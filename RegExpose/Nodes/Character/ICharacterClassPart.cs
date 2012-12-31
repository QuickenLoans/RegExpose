namespace RegExpose.Nodes.Character
{
    public interface ICharacterClassPart
    {
        bool Matches(char input);
    }
}