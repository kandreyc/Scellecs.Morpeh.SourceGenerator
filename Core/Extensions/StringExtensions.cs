namespace Scellecs.Morpeh.SourceGenerator.Core.Extensions;

public static class StringExtensions
{
    public static string FirstLetterLowerCase(this string str)
    {
        return string.IsNullOrEmpty(str)
            ? str
            : char.ToLower(str[0]) + str.Substring(1);
    }
}