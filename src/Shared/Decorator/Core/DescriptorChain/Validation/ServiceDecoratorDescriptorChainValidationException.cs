using System;
using System.Collections.Generic;
using System.Linq;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain.Validation;

internal class ServiceDecoratorDescriptorChainValidationException : Exception
{
    private const string MessageTemplate = "{0}Decorator descriptor chain error(s):{1}";

    internal IEnumerable<Exception> Errors { get; }

    internal ServiceDecoratorDescriptorChainValidationException(string? message, IEnumerable<Exception> errors)
        : base(ComposeMessage(message, errors), errors.FirstOrDefault())
    {
        Errors = errors;
    }

    private static string ComposeMessage(string? message, IEnumerable<Exception> errors)
    {
        var customComponent = message is not null ? $"{message} " : string.Empty;
        var errorsComponent = string.Join(string.Empty, errors.Select(error => $"{Environment.NewLine} - {error.Message}"));

        return string.Format(MessageTemplate, customComponent, errorsComponent);
    }
}
