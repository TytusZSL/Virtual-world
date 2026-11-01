using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Creatures;
using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Graphics
{
    public class DisplayableTile : ObservableObject
    {
        public DisplayableTile(Tile Tile)
        {
            this.Tile = Tile;
        }
        private Tile Tile { get; set; }

        public void UpdateTile(Tile newTile)
        {
            Tile = newTile;
            OnPropertyChanged(nameof(GraphicSrc));
        }

        public int X => Tile.X;
        public int Y => Tile.Y;
        public string GraphicSrc => Tile.Content?.GraphicSrc ?? "none.png";
    }
}
