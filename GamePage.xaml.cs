using Virtual_world.GameWorld.Environment;
using Virtual_world.GameWorld.Graphics;

namespace Virtual_world
{
    public partial class GamePage : ContentPage
    {
        public GamePage(DisplayWorld vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
