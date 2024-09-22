using Caesar.Mobile.Services;
using System.Windows.Input;

namespace Caesar.Mobile.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private string _username;
    private string _password;

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

    public ICommand LoginCommand { get; }

    public LoginViewModel(IApiService apiService)
    {
        _apiService = apiService;
        LoginCommand = new Command(OnLoginClicked);
    }

    private async void OnLoginClicked()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            await Shell.Current.DisplayAlert("Error", "Username or password is empty", "OK");
            return;
        }

        IsBusy = true;
        try
        {
            // Here you would typically call your API to authenticate
            // For now, let's just check if the username is "admin" and password is "password"
            if (Username == "admin" && Password == "password")
            {
                // Navigate to the main page
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Invalid username or password", "OK");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
