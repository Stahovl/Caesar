using Caesar.App.Interfaces;
using Caesar.App.ViewModels;
using Caesar.Core.Interfaces;
using System.Diagnostics;

namespace Caesar.App.Views;

public partial class MainPage : ContentPage
{
    private MainViewModel ViewModel => BindingContext as MainViewModel;

    public MainPage(IApiService apiService, ITokenService tokenService)
    {
        InitializeComponent();
        BindingContext = new MainViewModel(apiService, tokenService);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            await ViewModel.LoadData();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in OnAppearing: {ex.Message}");
            // Обработка исключения
        }
    }
}