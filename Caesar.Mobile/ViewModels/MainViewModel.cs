using Caesar.Core.DTOs;
using Caesar.Mobile.Services;
using System.Collections.ObjectModel;

namespace Caesar.Mobile.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IAuthService _authService;
    public ObservableCollection<MenuItemDto> MenuItems { get; set; }

    public MainViewModel(IApiService apiService, IAuthService authService)
    {
        _apiService = apiService;
        _authService = authService;
        MenuItems = new ObservableCollection<MenuItemDto>();
        LoadMenuItemsCommand = new Command(async () => await LoadMenuItems());
        LogoutCommand = new Command(async () => await Logout());
    }

    public Command LoadMenuItemsCommand { get; }
    public Command LogoutCommand { get; }

    private async Task LoadMenuItems()
    {
        if (!_authService.IsAuthenticated)
        {
            await Shell.Current.GoToAsync("//LoginPage");
            return;
        }

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

    private async Task Logout()
    {
        await _authService.ClearTokenAsync();
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
