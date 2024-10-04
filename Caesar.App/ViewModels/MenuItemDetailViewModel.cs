using Caesar.App.Interfaces;
using Caesar.Core.DTOs;
using System.Diagnostics;
using System.Windows.Input;

namespace Caesar.App.ViewModels;

[QueryProperty(nameof(ItemId), "id")]
public class MenuItemDetailViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private int _itemId;
    private string _name;
    private string _description;
    private decimal _price;
    private string _category;
    private string _imageUrl;

    public int ItemId
    {
        get => _itemId;
        set
        {
            _itemId = value;
            LoadMenuItem(_itemId);
        }
    }

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public decimal Price
    {
        get => _price;
        set => SetProperty(ref _price, value);
    }

    public string Category
    {
        get => _category;
        set => SetProperty(ref _category, value);
    }

    public string ImageUrl
    {
        get => _imageUrl;
        set => SetProperty(ref _imageUrl, value);
    }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public MenuItemDetailViewModel(IApiService apiService)
    {
        _apiService = apiService;
        SaveCommand = new Command(async () => await SaveMenuItem());
        CancelCommand = new Command(async () =>
        {
            Debug.WriteLine("Click cancel btn");
            await Shell.Current.GoToAsync("//MainPage");
        });
    }

    private async void LoadMenuItem(int itemId)
    {
        Debug.WriteLine($"LoadMenuItem called with itemId: {itemId}");
        if (itemId > 0)
        {
            try
            {
                var menuItem = await _apiService.GetMenuItemAsync(itemId);
                Debug.WriteLine($"MenuItem loaded: {System.Text.Json.JsonSerializer.Serialize(menuItem)}");
                Name = menuItem.Name;
                Description = menuItem.Description;
                Price = menuItem.Price;
                Category = menuItem.Category;
                ImageUrl = menuItem.ImageUrl;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading menu item: {ex}");
            }
        }
    }

    private async Task SaveMenuItem()
    {
        Debug.WriteLine("Click SaveMenuItem");
        try
        {
            var menuItemDto = new MenuItemDto
            {
                Id = ItemId,
                Name = Name,
                Description = Description,
                Price = Price,
                Category = Category,
                ImageUrl = ImageUrl
            };

            Debug.WriteLine($"Saving menu item: {System.Text.Json.JsonSerializer.Serialize(menuItemDto)}");

            bool result = await _apiService.SaveMenuItemAsync(menuItemDto);

            Debug.WriteLine($"Save result: {result}");

            if (result)
            {
                await Shell.Current.DisplayAlert("Success", "Menu item saved successfully", "OK");
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to save menu item", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in SaveMenuItem: {ex}");
            await Shell.Current.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
}
