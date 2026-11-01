using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
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

        public ObservableCollection<ObservableCollection<DisplayableTile>> TileRows { get; } = new();

        public void InitializeTilesCollection()
        {
            var world = World.Singleton;

            TileRows.Clear();
            for (int x = 0; x < world.Size; x++)
            {
                var row = new ObservableCollection<DisplayableTile>();
                for (int y = 0; y < world.Size; y++)
                {
                    row.Add(new(world.TileSet[x, y]));
                }
                TileRows.Add(row);
            }
        }

        public void UpdateTiles()
        {
            var world = World.Singleton;

            for (int x = 0; x < world.Size; x++)
            {
                for (int y = 0; y < world.Size; y++)
                {
                    TileRows[x][y].UpdateTile(world.TileSet[x, y]);
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
