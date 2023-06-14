using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using PathManager.Core.Contracts.Services;
using PathManager.UI.Models;
using static PathManager.Core.Contracts.Services.IEnvironmentService;

namespace PathManager.UI.ViewModels;

public class UserPathViewModel : ObservableRecipient
{
    public ObservableCollection<PathItem> PathItems
    {
        get;
    } = new();

    private readonly IPathService _pathService;

    private const Target Target = IEnvironmentService.Target.User;

    public UserPathViewModel(IPathService pathService)
    {
        _pathService = pathService;
        LoadPath();
        AddFromExplorerCommand = new AsyncRelayCommand(AddFromExplorer);
        AddFromClipboardCommand = new AsyncRelayCommand(AddFromClipboard);
        AddFromInputCommand = new RelayCommand(AddFromInput);
        LoadPathCommand = new RelayCommand(LoadPath);
    }

    private void LoadPath()
    {
        PathItems.Clear();
        var items = _pathService.GetItems(Target.User)
            .Select(i => new PathItem(i, CopyToClipboard, OpenExplorer, RemoveFromPath, Visibility.Visible));
        foreach (var pathItem in items)
        {
            PathItems.Add(pathItem);
        }
    }


    private async Task AddFromExplorer()
    {
        // https://aka.ms/cswinrt/interop#windows-sdk
        // ReSharper disable once IdentifierTypo
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow.CurrentWindow);
        var folderPicker = new FolderPicker
        {
            SuggestedStartLocation = PickerLocationId.ComputerFolder,
        };
        folderPicker.FileTypeFilter.Add("*");
        WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);
        var folder = await folderPicker.PickSingleFolderAsync();
        if (folder is not null && Directory.Exists(folder.Path))
        {
            _pathService.AddItem(folder.Path, Target);
            PathItems.Add(new PathItem(folder.Path, CopyToClipboard, OpenExplorer, RemoveFromPath, Visibility.Visible));
        }
    }

    private async Task AddFromClipboard()
    {
        var content = await Clipboard.GetContent().GetTextAsync();
        if (content is not null && Directory.Exists(content))
        {
            _pathService.AddItem(content, Target);
            PathItems.Add(new PathItem(content, CopyToClipboard, OpenExplorer, RemoveFromPath, Visibility.Visible));
        }
    }

    private void AddFromInput()
    {
        throw new NotImplementedException();
    }

    private static void CopyToClipboard(string directory)
    {
        var package = new DataPackage
        {
            RequestedOperation = DataPackageOperation.Copy
        };
        package.SetText(directory);
        Clipboard.SetContent(package);
    }

    private static void OpenExplorer(string directory) => Process.Start("explorer.exe", directory);

    private void RemoveFromPath(string directory)
    {
        _pathService.RemoveItem(directory, Target);
        PathItems.Remove(PathItems.First(i => i.Directory == directory));
    }


    public AsyncRelayCommand AddFromExplorerCommand
    {
        get;
    }

    public AsyncRelayCommand AddFromClipboardCommand
    {
        get;
    }

    public RelayCommand AddFromInputCommand
    {
        get;
    }

    public RelayCommand LoadPathCommand
    {
        get;
    }
}
