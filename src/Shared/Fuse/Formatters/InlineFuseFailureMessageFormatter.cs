using System;
using System.Text;

namespace TabAmp.Shared.Fuse.Formatters;

public readonly ref struct InlineFuseFailureMessageFormatter(string? message) : IFuseFailureMessageFormatter
{
    public string Format(FuseErrors errors)
    {
        var stringBuilder = new StringBuilder();

        FormatSummary(stringBuilder);

        foreach (var error in errors)
            FormatErrorLine(stringBuilder, error);

        return stringBuilder.ToString();
    }

    private void FormatSummary(StringBuilder stringBuilder)
    {
        if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message))
            return;

        stringBuilder.Append(message);
        stringBuilder.Append(' ');
    }

    private static void FormatErrorLine(StringBuilder stringBuilder, Exception error)
    {
        stringBuilder.Append(error.Message);
        stringBuilder.Append(' ');
    }
}
