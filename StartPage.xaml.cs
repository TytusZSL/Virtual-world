using Virtual_world.ViewModels;

namespace Virtual_world;

public partial class StartPage : ContentPage
{
	public StartPage(StartViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}