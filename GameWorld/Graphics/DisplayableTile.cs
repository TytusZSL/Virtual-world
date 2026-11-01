using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Creatures;
using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Graphics
{
    public partial class DisplayableTile : ObservableObject
    {
        public DisplayableTile(Tile Tile)
        {
            this.Tile = Tile;
        }
        private Tile Tile { get; set; }

        [ObservableProperty]
        private bool _isSelected;

        partial void OnIsSelectedChanged(bool value) => OnPropertyChanged(nameof(BorderColor));

        public void UpdateTile(Tile newTile, bool isSelected)
        {
            Tile = newTile;
            IsSelected = isSelected;
            OnPropertyChanged(nameof(GraphicSrc));
        }

        public int X => Tile.X;
        public int Y => Tile.Y;
        public string GraphicSrc => Tile.Content?.GraphicSrc ?? "none.png";
        public Color BorderColor => IsSelected ? Colors.Yellow : Colors.Black;
    }
}
