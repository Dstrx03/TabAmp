using System;
using System.Text;

namespace TabAmp.Shared.Validation.Formatters;

public readonly ref struct MultilineValidationExceptionMessageFormatter(string? message) : IValidationExceptionMessageFormatter
{
    private const string DefaultSummary = "TODO...";

    public string Format(Errors errors)
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
