using Caesar.Core.DTOs;
using Caesar.App.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace Caesar.App.ViewModels;

public class MenuItemViewModel : INotifyPropertyChanged
{
    private readonly IApiService _apiService;

    private int _id;
    private string _name;
    private string _description;
    private decimal _price;
    private string _category;

    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged(nameof(Id));
        }
    }

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

    public string Category
    {
        get => _category;
        set
        {
            _category = value;
            OnPropertyChanged(nameof(Category));
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
        try
        {
            var menuItemDto = ToDto();
            var result = await _apiService.SaveMenuItemAsync(menuItemDto);
            if (result)
            {
                // Обработка успешного сохранения
                await Shell.Current.DisplayAlert("Success", "Menu item saved successfully", "OK");
            }
            else
            {
                // Обработка неудачного сохранения
                await Shell.Current.DisplayAlert("Error", "Failed to save menu item", "OK");
            }
        }
        catch (Exception ex)
        {
            // Обработка исключений
            await Shell.Current.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private MenuItemDto ToDto()
    {
        return new MenuItemDto
        {
            Id = this.Id,
            Name = this.Name,
            Description = this.Description,
            Price = this.Price,
            Category = this.Category
        };
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
