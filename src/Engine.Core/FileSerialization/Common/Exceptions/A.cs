namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions;

internal  class A: FileSerializationException
{
    public A(int count)
        : base($"Unable to read the next {count} byte(s), the specified byte count must be a non-negative value.")
    {
    }
}
