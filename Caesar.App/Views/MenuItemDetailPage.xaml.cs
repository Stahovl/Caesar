using Caesar.App.Services;
using Caesar.App.ViewModels;

namespace Caesar.App.Views;

public partial class MenuItemDetailPage : ContentPage
{
	public MenuItemDetailPage(IApiService apiService)
	{
		InitializeComponent();
		BindingContext = new MenuItemDetailViewModel(apiService);
	}
}