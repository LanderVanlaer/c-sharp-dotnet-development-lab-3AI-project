using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using desktopapplication.Models;
using desktopapplication.services;
using desktopapplication.services.api;

namespace desktopapplication.ViewModels;

public class AddPaymentPageViewModel : BaseViewModel
{
    private readonly Group _group;
    private string _description = String.Empty;
    private string _name = String.Empty;
    private ObservableCollection<PaymentEntry> _paymentEntriesHasToPay = [];
    private ObservableCollection<PaymentEntry> _paymentEntriesPayed = [];

    private int _paymentTypeIndex;

    public AddPaymentPageViewModel(Group group)
    {
        _group = group;
        PaymentTypeIndex = 0;

        ResetPaymentEntriesHasToPayCommand = new Command(ResetPaymentEntriesHasToPay);

        SaveCommand = new Command(Save);

        LoadOnTask(Repository.FetchUsers(group.Id))
            .ContinueWith(task =>
            {
                PaymentEntriesPayed = new ObservableCollection<PaymentEntry>(
                    task.Result.Select(user =>
                    {
                        PaymentEntry paymentEntry = new(user);

                        paymentEntry.PropertyChanged += (_, args) =>
                        {
                            if (args.PropertyName == nameof(PaymentEntry.Amount)) AutoSetAmounts();
                        };

                        paymentEntry.PropertyChanged += (_, _) =>
                        {
                            OnPropertyChanged(nameof(SumOfPaymentEntriesPayed));
                            OnPropertyChanged(nameof(CanSave));
                        };

                        return paymentEntry;
                    })
                );

                PaymentEntriesHasToPay = new ObservableCollection<PaymentEntry>(
                    task.Result.Select(user =>
                    {
                        PaymentEntry paymentEntry = new(user);

                        paymentEntry.PropertyChanged += (_, _) =>
                        {
                            OnPropertyChanged(nameof(SumOfPaymentEntriesHasToPay));
                            OnPropertyChanged(nameof(CanSave));
                        };

                        return paymentEntry;
                    })
                );
            });
    }

    public Command SaveCommand { get; }

    public decimal SumOfPaymentEntriesPayed => PaymentEntriesPayed.Sum(entry => entry.Amount);
    public decimal SumOfPaymentEntriesHasToPay => PaymentEntriesHasToPay.Sum(entry => entry.Amount);

    public bool CanSave =>
        SumOfPaymentEntriesPayed > 0 &&
        Math.Abs(SumOfPaymentEntriesPayed - SumOfPaymentEntriesHasToPay) < 0.01m;

    public string Title => "Add Payment to " + _group.Name;

    public ObservableCollection<PaymentEntry> PaymentEntriesPayed
    {
        get => _paymentEntriesPayed;
        set => SetField(ref _paymentEntriesPayed, value);
    }

    public ObservableCollection<PaymentEntry> PaymentEntriesHasToPay
    {
        get => _paymentEntriesHasToPay;
        set => SetField(ref _paymentEntriesHasToPay, value);
    }


    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    public string Description
    {
        get => _description;
        set => SetField(ref _description, value);
    }

    public int PaymentTypeIndex
    {
        get => _paymentTypeIndex;
        set => SetField(ref _paymentTypeIndex, value);
    }

    public Command ResetPaymentEntriesHasToPayCommand { get; }

    private async void Save()
    {
        if (!CanSave) return;

        if (Name == String.Empty)
        {
            await Shell.Current.DisplayAlert("Error", "Name cannot be empty", "OK");
            return;
        }

        try
        {
            await LoadOnTask(Repository.AddPayment(
                _group.Id,
                PaymentTypeIndex switch
                {
                    0 => Payment.PaymentType.Purchase,
                    1 => Payment.PaymentType.Repayment,
                    _ => throw new Exception(),
                },
                Name,
                Description,
                PaymentEntriesPayed
                    .Where(entry => entry.Amount > 0)
                    .Select(entry => new services.PaymentEntry(entry.User.Id, entry.Amount))
                    .Concat(PaymentEntriesHasToPay
                        .Where(entry => entry.Amount > 0)
                        .Select(entry => new services.PaymentEntry(entry.User.Id, -entry.Amount)
                        ))
                    .GroupBy(
                        entry => entry.UserId,
                        entry => entry.Amount,
                        (guid, amounts) => new services.PaymentEntry(guid, amounts.Sum())
                    ).Where(entry => entry.Amount != 0)
            ));

            await Shell.Current.Navigation.PopAsync();
        }
        catch (ApiError e)
        {
            await Shell.Current.DisplayAlert("Error",
                e.Body?.Errors != null && e.Body.Errors.Count != 0 ? string.Join("\n", e.Body.Errors) : e.Message,
                "OK");
        }
    }

    public void ResetPaymentEntriesHasToPay()
    {
        foreach (PaymentEntry paymentEntry in PaymentEntriesHasToPay) paymentEntry.Amount = 0;
    }

    private void AutoSetAmounts()
    {
        decimal totalAmount = PaymentEntriesPayed.Sum(entry => entry.Amount);
        decimal amountPerUser = totalAmount / PaymentEntriesHasToPay.Count;

        foreach (PaymentEntry entry in PaymentEntriesHasToPay) entry.Amount = amountPerUser;
    }
}

public class PaymentEntry(User user) : INotifyPropertyChanged
{
    private decimal _amount;
    private string _amountAsString = "0";

    public User User
    {
        get => user;
        set
        {
            SetField(ref user, value);
            OnPropertyChanged(nameof(Username));
        }
    }

    public string Username => User.Username;

    public decimal Amount
    {
        get => _amount;
        set
        {
            if (value == _amount || value < 0) return;

            SetField(ref _amount, value);

            AmountAsString = Amount.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string AmountAsString
    {
        get => _amountAsString;
        set
        {
            if (value.Length != 1)
                value = value.TrimStart('0');

            if (value.Count(c => c == '.') > 1)
            {
                int indexFirstOccurence = value.IndexOf('.');
                value = string.Concat(
                    value.AsSpan(0, indexFirstOccurence),
                    ".",
                    value[indexFirstOccurence..].Replace(".", string.Empty)
                );
            }

            if (value == AmountAsString) return;
            if (value == "0" && AmountAsString == ".") return;

            if (decimal.TryParse(value, out decimal res2) &&
                decimal.TryParse(AmountAsString, out decimal res) &&
                res == res2
               ) return;

            if (value == String.Empty)
            {
                Amount = 0;
                SetField(ref _amountAsString, "0");
                return;
            }

            value = value.Replace(',', '.');

            if (value == ".")
            {
                Amount = 0;
                SetField(ref _amountAsString, value);
                return;
            }

            if (decimal.TryParse(value.EndsWith('.') ? value[..^1] : value, out decimal result))
            {
                Amount = result;
                SetField(ref _amountAsString, value);
            }
            else
            {
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}