using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Virtual_world.GameWorld.Creatures;
using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Graphics
{
    public partial class DisplayWorld : ObservableObject
    {
        public DisplayWorld()
        {
            Singleton = this;
        }

        public static DisplayWorld Singleton = new DisplayWorld();

        public ObservableCollection<DisplayableTile> Tiles { get; } = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ExpectedWorldWidth))]
        private int worldWidth;
        public int ExpectedWorldWidth => WorldWidth * 54 + 5;

        public void UpdateTilesCollection()
        {
            var world = World.Singleton;
            Tiles.Clear();
            for (int x = 0; x < world.SizeX; x++)
            {
                for (int y = 0; y < world.SizeY; y++)
                {
                    Tiles.Add(new(world.TileSet[x, y]));
                }
            }
            WorldWidth = world.SizeX;
        }
    }
}
