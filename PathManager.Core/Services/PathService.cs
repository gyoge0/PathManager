using System.Security;
using PathManager.Core.Contracts.Services;

namespace PathManager.Core.Services;

public class PathService : IPathService
{
    private IFileService _fileService;
    private IEnvironmentService _environmentService;

    public PathService(IFileService fileService, IEnvironmentService environmentService)
    {
        _fileService = fileService;
        _environmentService = environmentService;
    }

    public bool AddItem(string path, IEnvironmentService.Target target)
    {
        if (!_fileService.DirectoryExists(path))
        {
            return true;
        }
        
        var currentPath = GetItems(target);
        var normalized = Normalize(path);
        if (currentPath.Any(current => Normalize(current) == normalized))
        {
            return true;
        }

        currentPath.Add(path);
        try
        {
            _environmentService.SetVariable("Path", string.Join(";", currentPath), target);
            return true;
        }
        catch (SecurityException)
        {
            return false;
        }
    }

    public bool RemoveItem(string path, IEnvironmentService.Target target)
    {
        if (!_fileService.DirectoryExists(path))
        {
            return true;
        }
        var currentPath = GetItems(target);
        var normalized = Normalize(path);
        if (currentPath.All(current => Normalize(current) != normalized))
        {
            return true;
        }

        currentPath.RemoveAll(i => Normalize(i) == normalized);
        try
        {
            _environmentService.SetVariable("Path", string.Join(";", currentPath), target);
            return true;
        }
        catch (SecurityException)
        {
            return false;
        }
    }

    public List<string> GetItems(IEnvironmentService.Target from) => _environmentService
        .GetVariable("Path", from)
        ?.Split(";")
        .Where(p => _fileService.DirectoryExists(p))
        .ToList() ?? new List<string>();

    private static string Normalize(string path)
    {
        var tmp = path.Trim().ToLower().Replace('/', '\\');
        if (tmp.EndsWith("\\"))
        {
            tmp = tmp.Substring(0, tmp.Length - 1);
        }

        return tmp;
    }
}
