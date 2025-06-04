namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation.Fluent;

internal readonly record struct OperationExceptionFluentBuilder<TException>(Operation Operation)
    where TException : OperationException;
