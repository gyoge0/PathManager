using static PathManager.Core.Contracts.Services.IEnvironmentService;

namespace PathManager.Core.Contracts.Services;

public interface IPathService
{
    bool AddItem(string path, Target target);
    bool RemoveItem(string path, Target target);
    List<string> GetItems(Target from);

}
