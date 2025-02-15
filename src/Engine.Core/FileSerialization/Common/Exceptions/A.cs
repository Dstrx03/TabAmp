namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions;

internal  class A: FileSerializationException
{
    public A(int count)
        : base($"Unable to read next {count} byte(s).")
    {
    }
}
