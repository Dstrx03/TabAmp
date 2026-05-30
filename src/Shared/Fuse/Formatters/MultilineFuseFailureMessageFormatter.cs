using System;
using System.Text;

namespace TabAmp.Shared.Fuse.Formatters;

public readonly ref struct MultilineFuseFailureMessageFormatter(string? message) : IFuseFailureMessageFormatter
{
    private const string DefaultSummary = "The following error(s) occurred:";

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
        var summary = DefaultSummary;

        if (!string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message))
            summary = message;

        stringBuilder.Append(summary);
    }

    private static void FormatErrorLine(StringBuilder stringBuilder, Exception error)
    {
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(" - ");
        stringBuilder.Append(error.Message);
    }
}
