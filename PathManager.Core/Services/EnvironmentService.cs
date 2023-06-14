using PathManager.Core.Contracts.Services;

namespace PathManager.Core.Services;

public class EnvironmentService : IEnvironmentService
{
    public void SetVariable(string name, string? value, IEnvironmentService.Target target) =>
        Environment.SetEnvironmentVariable(name, value, target switch
        {
            IEnvironmentService.Target.System => EnvironmentVariableTarget.Machine,
            IEnvironmentService.Target.User => EnvironmentVariableTarget.User,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        });

    public string? GetVariable(string name, IEnvironmentService.Target target) =>
        Environment.GetEnvironmentVariable(name, target switch
        {
            IEnvironmentService.Target.System => EnvironmentVariableTarget.Machine,
            IEnvironmentService.Target.User => EnvironmentVariableTarget.User,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        });
}
