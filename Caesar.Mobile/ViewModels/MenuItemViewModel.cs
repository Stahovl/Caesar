using Caesar.Mobile.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace Caesar.Mobile.ViewModels;

public class MenuItemViewModel : INotifyPropertyChanged
{
    private readonly IApiService _apiService;

    private string _name;
    private string _description;
    private decimal _price;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged(nameof(Description));
        }
    }

    public decimal Price
    {
        get => _price;
        set
        {
            _price = value;
            OnPropertyChanged(nameof(Price));
        }
    }

    public ICommand SaveCommand { get; }

    public MenuItemViewModel(IApiService apiService)
    {
        _apiService = apiService;
        SaveCommand = new Command(async () => await SaveMenuItem());
    }

    private async Task SaveMenuItem()
    {
        // Здесь будет логика сохранения пункта меню через API
        await _apiService.SaveMenuItemAsync(this);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
