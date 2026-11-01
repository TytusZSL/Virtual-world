using Virtual_world.GameWorld.Graphics;

namespace Virtual_world
{
    public partial class GamePage : ContentPage
    {
        private const int TileSize = 54;
        private const int FixedPaddingX = 15;
        private const int FixedPaddingY = 40;
        private const int MaximumWindowSize = 1000;
        private Window? _gameWindow;
        private Window? _actionsWindow;
        private bool _closingWindows;

        public GamePage(DisplayWorld vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        private void OnTileClicked(object? sender, EventArgs e)
        {
            if (sender is Button { CommandParameter: DisplayableTile tile })
                ((DisplayWorld)BindingContext).SelectTile(tile);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_actionsWindow is not null)
            {
                return;
            }

            var application = Application.Current ?? throw new InvalidOperationException("The application is not available.");
            if (application.Windows.Count == 0)
            {
                throw new InvalidOperationException("The game page is not attached to an application window.");
            }

            _gameWindow = application.Windows[0];
            var gridSize = ((DisplayWorld)BindingContext).TileRows.Count * TileSize;
            _gameWindow.Width = Math.Min(gridSize + FixedPaddingX, MaximumWindowSize);
            _gameWindow.Height = Math.Min(gridSize + FixedPaddingY, MaximumWindowSize);
            _gameWindow.Destroying += OnGameWindowDestroying;

            _actionsWindow = new Window(new ActionsPage((DisplayWorld)BindingContext))
            {
                Width = 600,
                Height = 1000
            };
            _actionsWindow.Destroying += OnActionsWindowDestroying;
            application.OpenWindow(_actionsWindow);
        }

        protected override void OnDisappearing()
        {
            CloseActionsWindow();
            if (_gameWindow is not null)
            {
                _gameWindow.Destroying -= OnGameWindowDestroying;
                _gameWindow = null;
            }

            base.OnDisappearing();
        }

        private void OnGameWindowDestroying(object? sender, EventArgs e)
        {
            if (_closingWindows)
            {
                return;
            }

            _closingWindows = true;
            CloseActionsWindow();
        }

        private void OnActionsWindowDestroying(object? sender, EventArgs e)
        {
            if (_closingWindows)
            {
                return;
            }

            _closingWindows = true;
            if (_gameWindow is not null)
            {
                Application.Current?.CloseWindow(_gameWindow);
            }
        }

        private void CloseActionsWindow()
        {
            if (_actionsWindow is not { } actionsWindow)
            {
                return;
            }

            actionsWindow.Destroying -= OnActionsWindowDestroying;
            _actionsWindow = null;
            Application.Current?.CloseWindow(actionsWindow);
        }
    }
}
