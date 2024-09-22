using Caesar.Core.DTOs;
using Caesar.Mobile.Services;
using System.Collections.ObjectModel;

namespace Caesar.Mobile.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    public ObservableCollection<MenuItemDto> MenuItems { get; set; }

    public MainViewModel(IApiService apiService)
    {
        _apiService = apiService;
        MenuItems = new ObservableCollection<MenuItemDto>();
        LoadMenuItemsCommand = new Command(async () => await LoadMenuItems());
    }

    public Command LoadMenuItemsCommand { get; }

    private async Task LoadMenuItems()
    {
        IsBusy = true;

        try
        {
            MenuItems.Clear();
            var items = await _apiService.GetMenuItemsAsync();
            foreach (var item in items)
            {
                MenuItems.Add(item);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Unable to load menu items: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
