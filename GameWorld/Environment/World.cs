using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Creatures;
using Virtual_world.GameWorld.Creatures.Animals;
using Virtual_world.GameWorld.Graphics;

namespace Virtual_world.GameWorld.Environment
{
    public class World
    {
        public World(int Size)
        {
            this.Size = Size;
            TileSet = new Tile[Size, Size];
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    TileSet[x, y] = new Tile(x, y);
                }
            }
            TileSet[1, 1].Content = new Wolf(1, 1);
            Singleton = this;
            DisplayWorld.Singleton.InitializeTilesCollection();
        }
        public void DoTurn()
        {
            List<Organism> organisms = new();
            foreach (var tile in TileSet)
            {
                if (tile.Content != null)
                {
                    organisms.Add(tile.Content);
                }
            }
            var sortedOrganisms = organisms
                .OrderByDescending(o => o.Initiative)
                .ThenByDescending(o => o.Age);

            foreach (var organism in sortedOrganisms)
            {
                organism.Action();
            }

            DisplayWorld.Singleton.UpdateTiles();
        }
        public void DrawWorld() { }

        public readonly int Size;

        public static World Singleton = new World(20);

        public Tile[,] TileSet;

        public Random rngGen = new Random();
    }
}
