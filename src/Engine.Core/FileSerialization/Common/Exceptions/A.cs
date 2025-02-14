namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions;

internal  class A: FileSerializationException
{
    public A(int count)
        : base($"Cannot read {count} byte(s).")
    {
    }
}
