using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;

namespace PathManager.UI.Models;

public class PathItem
{
    public PathItem(
        string directory,
        Action<string> copyToClipboard,
        Action<string> openExplorer,
        Action<string> removeFromPath,
        Visibility visible
    )
    {
        Directory = directory;
        CopyToClipboardCommand = new RelayCommand(() => copyToClipboard(directory));
        OpenExplorerCommand = new RelayCommand(() => openExplorer(directory));
        RemoveFromPathCommand = new RelayCommand(() => removeFromPath(directory));
        Visible = visible;
    }

    public string Directory
    {
        get;
        set;
    }

    public ICommand CopyToClipboardCommand
    {
        get;
    }

    public ICommand OpenExplorerCommand
    {
        get;
    }

    public ICommand RemoveFromPathCommand
    {
        get;
    }

    public Visibility Visible
    {
        get;
    }
}
