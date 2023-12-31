﻿using System.ComponentModel;
using desktopapplication.Models;
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

    private async void GroupsListView_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Group group)
            await Navigation.PushAsync(new PaymentsPage(group));

        GroupsListView.SelectedItem = null;
    }

    private void CreateGroup_OnClicked(object? sender, EventArgs e) => Navigation.PushAsync(new CreateGroupPage());
}