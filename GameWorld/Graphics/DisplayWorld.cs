using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        private bool Initialized = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ExpectedWorldWidth))]
        public partial int WorldWidth { get; set; }
        public int ExpectedWorldWidth => WorldWidth * 54 + 5;

        public void InitializeTilesCollection()
        {
            var world = World.Singleton;

            Tiles.Clear();
            for (int x = 0; x < world.Size; x++)
            {
                for (int y = 0; y < world.Size; y++)
                {
                    Tiles.Add(new(world.TileSet[x, y]));
                }
            }
            WorldWidth = world.Size;
        }

        public void UpdateTiles()
        {
            var world = World.Singleton;

            for (int x = 0; x < world.Size; x++)
            {
                for (int y = 0; y < world.Size; y++)
                {
                    Tiles[x * world.Size + y] = new(world.TileSet[x, y]);
                }
            }
        }

        [RelayCommand]
        public void DoTurn()
        {
            World.Singleton.DoTurn();
        }
    }
}
