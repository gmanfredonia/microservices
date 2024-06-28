namespace Services.Abstractions;

public interface IServiceManager
{
    public IServiceSecurity ServiceSecurity { get; }
    public IServiceProducts ServiceProducts { get; }
}