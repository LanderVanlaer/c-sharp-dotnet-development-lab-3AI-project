using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using desktopapplication.services;

namespace desktopapplication.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    protected readonly IRepository Repository = DependencyService.Get<IRepository>();
    private bool _isBusy;


    public bool IsBusy
    {
        get => _isBusy;
        set => SetField(ref _isBusy, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public async Task<T> LoadOnTask<T>(Task<T> task)
    {
        StartLoading();
        try
        {
            return await task;
        }
        finally
        {
            StopLoading();
        }
    }

    public async Task LoadOnTask(Task task)
    {
        StartLoading();

        try
        {
            await task;
        }
        finally
        {
            StopLoading();
        }
    }

    public void StartLoading() => IsBusy = true;
    public void StopLoading() => IsBusy = false;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected static async Task DisplayAlert(string title, string message, string cancel)
    {
        Debug.WriteLine("DisplayAlert called " + title + " " + message + " " + cancel);
        if (Application.Current?.MainPage is not null)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
            return;
        }

        await Shell.Current.DisplayAlert(title, message, cancel);
    }
}