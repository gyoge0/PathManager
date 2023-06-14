using Microsoft.UI.Xaml.Controls;
using PathManager.UI.ViewModels;

namespace PathManager.UI.Views;

public sealed partial class UserPathPage : Page
{
    public UserPathViewModel ViewModel
    {
        get;
        set;
    }

    public UserPathPage()
    {
        ViewModel = App.GetService<UserPathViewModel>();
        InitializeComponent();
    }
}
