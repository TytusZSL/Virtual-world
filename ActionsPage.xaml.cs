using Virtual_world.GameWorld.Graphics;

namespace Virtual_world;

public partial class ActionsPage : ContentPage
{
    public ActionsPage(DisplayWorld viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
