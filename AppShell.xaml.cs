namespace Virtual_world
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("GamePage", typeof(GamePage));
        }
    }
}
