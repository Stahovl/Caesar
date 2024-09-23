using System.Diagnostics;
using System.Windows.Input;
using Caesar.App.Services;

namespace Caesar.App.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly ITokenService _tokenService;

    private string _username;
    private string _password;
    private string _errorMessage;

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel(IApiService apiService, ITokenService tokenService)
    {
        _apiService = apiService;
        _tokenService = tokenService;
        LoginCommand = new Command(async () => await OnLoginClicked());
    }

    private async Task OnLoginClicked()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Username or password is empty", "OK");
            return;
        }

        try
        {
            var result = await _apiService.LoginAsync(Username, Password);
            if (result.IsSuccess)
            {
                await _tokenService.SetTokenAsync(result.Token);
                Debug.WriteLine($"Token saved: {result.Token.Substring(0, 10)}..."); // Логируем только часть токена для безопасности
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid username or password", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Login error: {ex.Message}");
            await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred during login: {ex.Message}", "OK");
        }
    }
}
