namespace TabAmp.Shared.Validation;

public readonly ref struct Scope
{
    private readonly Context _context;

    private Scope(Context context)
    {
        _context = context;
    }

    public static Scope Init_TODONAME(bool stopOnFirstError) => new(Context.Init_TODONAME(stopOnFirstError: stopOnFirstError));
}
