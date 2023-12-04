using System.ComponentModel;
using desktopapplication.ViewModels;

namespace desktopapplication.Views;

public partial class GroupsView : ContentView, INotifyPropertyChanged
{
    private GroupsViewModel _viewModel;

    public GroupsView()
    {
        InitializeComponent();
        BindingContext = _viewModel = new GroupsViewModel();
    }
}