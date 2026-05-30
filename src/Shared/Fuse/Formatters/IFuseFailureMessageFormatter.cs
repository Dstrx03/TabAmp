namespace TabAmp.Shared.Fuse.Formatters;

public interface IFuseFailureMessageFormatter
{
    string Format(FuseErrors errors);
}
