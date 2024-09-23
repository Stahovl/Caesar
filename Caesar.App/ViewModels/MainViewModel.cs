using Caesar.Core.DTOs;
using Caesar.App.Services;
using System.Collections.ObjectModel;
using Caesar.Core.Interfaces;
using System.Windows.Input;
using System.Diagnostics;

namespace Caesar.App.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly ITokenService _tokenService;

    public ObservableCollection<MenuItemDto> MenuItems { get; set; }
    public ObservableCollection<ReservationDto> Reservations { get; set; }

    private MenuItemDto _selectedMenuItem;
    public MenuItemDto SelectedMenuItem
    {
        get => _selectedMenuItem;
        set => SetProperty(ref _selectedMenuItem, value);
    }

    public ICommand LoadMenuItemsCommand { get; }
    public ICommand LoadReservationsCommand { get; }
    public ICommand AddMenuItemCommand { get; }
    public ICommand EditMenuItemCommand { get; }
    public ICommand DeleteMenuItemCommand { get; }
    public ICommand LogoutCommand { get; }

    public MainViewModel(IApiService apiService, ITokenService tokenService)
    {
        _apiService = apiService;
        _tokenService = tokenService;

        MenuItems = new ObservableCollection<MenuItemDto>();
        Reservations = new ObservableCollection<ReservationDto>();

        LoadMenuItemsCommand = new Command(async () => await LoadMenuItems());
        LoadReservationsCommand = new Command(async () => await LoadReservations());

        AddMenuItemCommand = new Command(async () => await AddMenuItem());
        DeleteMenuItemCommand = new Command<int>(async (id) => await DeleteMenuItem(id));
        EditMenuItemCommand = new Command<int>(async (id) => await EditMenuItem(id));

        LogoutCommand = new Command(async () => await Logout());

    }

    public async Task LoadData()
    {
        Debug.WriteLine("Starting LoadData");
        try
        {
            bool isAuthenticated = await _tokenService.IsAuthenticatedAsync();
            Debug.WriteLine($"IsAuthenticated: {isAuthenticated}");

            if (!isAuthenticated)
            {
                Debug.WriteLine("Not authenticated, navigating to LoginPage");
                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }

            Debug.WriteLine("Authenticated, loading menu items");
            await LoadMenuItems();
            Debug.WriteLine("Loading reservations");
            await LoadReservations();
            Debug.WriteLine("LoadData completed");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in LoadData: {ex.Message}");
        }
    }

    private async Task LoadMenuItems()
    {
        try
        {
            Debug.WriteLine("Starting LoadMenuItems");
            IsBusy = true;
            var items = await _apiService.GetMenuItemsAsync();
            Debug.WriteLine($"Received {items.Count()} menu items");
            MenuItems.Clear();
            foreach (var item in items)
            {
                MenuItems.Add(item);
            }
            Debug.WriteLine("Finished adding menu items to collection");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading menu items: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", $"Unable to load menu items: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task LoadReservations()
    {
        try
        {
            IsBusy = true;
            var reservations = await _apiService.GetReservationsAsync();
            Reservations.Clear();
            foreach (var reservation in reservations)
            {
                Reservations.Add(reservation);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading reservations: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", $"Unable to load reservations: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task AddMenuItem()
    {
        await Shell.Current.GoToAsync("//MenuItemDetailPage");
    }

    private async Task EditMenuItem(int id)
    {
        await Shell.Current.GoToAsync($"//MenuItemDetailPage?id={id}");
    }

    private async Task DeleteMenuItem(int id)
    {
        bool confirmed = await Application.Current.MainPage.DisplayAlert("Confirm Delete", "Are you sure you want to delete this item?", "Yes", "No");
        if (confirmed)
        {
            bool success = await _apiService.DeleteMenuItemAsync(id);
            if (success)
            {
                var itemToRemove = MenuItems.FirstOrDefault(item => item.Id == id);
                if (itemToRemove != null)
                {
                    MenuItems.Remove(itemToRemove);
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete the item", "OK");
            }
        }
    }

    private async Task Logout()
    {
        await _tokenService.ClearTokenAsync();
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
