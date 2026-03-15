using System;
using System.Collections.Generic;
using System.Linq;

namespace TabAmp.Shared.Validation;

public class ValidationException : Exception
{
    private const string MessageTemplate = "{0}Decorator descriptor chain error(s):{1}";

    public IEnumerable<Exception> Errors { get; }

    internal ValidationException(string? message, IEnumerable<Exception> errors)
        : base(ComposeMessage(message, errors), errors.FirstOrDefault())
    {
        Errors = errors;
    }

    private static string ComposeMessage(string? message, IEnumerable<Exception> errors)
    {
        var useCustomComponent = !string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message);

        var customComponent = useCustomComponent ? $"{message} " : string.Empty;
        var errorsComponent = string.Join(string.Empty, errors.Select(error => $"{Environment.NewLine} - {error.Message}"));

        return string.Format(MessageTemplate, customComponent, errorsComponent);
    }
}
