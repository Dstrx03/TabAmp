namespace TabAmp.Shared.Validation.Formatters;

public interface IValidationExceptionMessageFormatter
{
    string Format(Errors errors);
}
