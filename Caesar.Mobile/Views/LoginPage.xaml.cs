using Caesar.Mobile.Services;
using Caesar.Mobile.ViewModels;

namespace Caesar.Mobile.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(IApiService apiService, IAuthService authService)
	{
		InitializeComponent();
        BindingContext = new LoginViewModel(apiService, authService);
    }
}