namespace BuildingBase.Exceptions;

public class ModelStateError : Exception
{

    public ModelStateError(in string field, in string message, params string[] parms)
        => Errors = [(field, message, parms)];
    public ModelStateError(in (string, string, string[])[] errors)
        => Errors = errors;

    public ModelStateError(in string message, params string[] parms)
        => Errors = [(string.Empty, message, parms)];
    public ModelStateError(in (string, string[])[] errors)
    {
        (string key, string message, string[] parms)[] data = new (string, string, string[])[errors.Length];
        for (int i = 0; i < data.Length; i++)
            data[i] = (string.Empty, data[i].key, data[i].parms);
        Errors = data;
    }    

    public (string, string, string[])[] Errors { get; }
}