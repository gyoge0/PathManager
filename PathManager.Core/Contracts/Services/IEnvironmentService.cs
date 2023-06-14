namespace PathManager.Core.Contracts.Services;

public interface IEnvironmentService
{
    void SetVariable(string name, string? value, Target target);
    string? GetVariable(string name, Target target);
    
    public enum Target
    {
        System,
        User
    }
}
