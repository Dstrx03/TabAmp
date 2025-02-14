namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions;

internal class B : FileSerializationException
{
    public B(int count)
        : base($"Cannot skip {count} byte(s).")
    {
    }
}
