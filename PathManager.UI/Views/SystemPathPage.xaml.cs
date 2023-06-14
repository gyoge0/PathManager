using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PathManager.UI.ViewModels;

namespace PathManager.UI.Views;

public sealed partial class SystemPathPage : Page
{
    private const string Install = @"\microsoft\windowsapps\pathmgr.exe";

    public SystemPathViewModel ViewModel
    {
        get;
        set;
    }

    public SystemPathPage()
    {
        ViewModel = App.GetService<SystemPathViewModel>();
        InitializeComponent();
    }

    private void Elevate(object sender, RoutedEventArgs e)
    {
        var aliasPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Install;
        var info = new ProcessStartInfo
        {
            Verb = "runas",
            UseShellExecute = true,
            FileName = aliasPath
        };
        Process.Start(info);

        Application.Current.Exit();
    }
}
