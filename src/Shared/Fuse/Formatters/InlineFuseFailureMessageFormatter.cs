using System.Text;

namespace TabAmp.Shared.Fuse.Formatters;

public readonly ref struct InlineFuseFailureMessageFormatter(string? message) : IFuseFailureMessageFormatter
{
    public string Format(FuseErrors errors)
    {
        var stringBuilder = new StringBuilder();

        FormatSummary(stringBuilder);
        FormatErrorLine(stringBuilder, errors);

        return stringBuilder.ToString();
    }

    private void FormatSummary(StringBuilder stringBuilder)
    {
        if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message))
            return;

        stringBuilder.Append(message);
        stringBuilder.Append(' ');
    }

    private static void FormatErrorLine(StringBuilder stringBuilder, FuseErrors errors)
    {
        var isFirst = true;
        foreach (var error in errors)
        {
            if (isFirst) isFirst = false;
            else stringBuilder.Append(' ');

            stringBuilder.Append(error.Message);
        }
    }
}
