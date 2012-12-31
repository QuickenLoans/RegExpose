using RegExpose.Nodes.Character;

namespace RegExpose
{
    internal static class ExtensionMethods
    {
        public static char FixCase(this ICharacterMatcher characterMatcher, char c)
        {
            if (characterMatcher.IgnoreCase)
            {
                return char.ToLowerInvariant(c);
            }

            return c;
        }
    }
}