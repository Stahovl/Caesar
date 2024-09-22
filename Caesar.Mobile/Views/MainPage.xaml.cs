using Caesar.Mobile.Services;
using Caesar.Mobile.ViewModels;

namespace Caesar.Mobile.Views;

public partial class MainPage : ContentPage
{
	public MainPage(IApiService apiService)
	{
        InitializeComponent();
		BindingContext = new MainViewModel(apiService);
	}
}