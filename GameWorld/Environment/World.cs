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
        public World(int size) : this(size, DisplayWorld.Singleton)
        {
        }

        public World(int size, DisplayWorld displayWorld)
        {
            Size = size;
            TileSet = new Tile[size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    TileSet[x, y] = new Tile(x, y);
                }
            }
            if (size > 1)
            {
                TileSet[1, 1].Content = new Wolf(1, 1);
            }
            Singleton = this;
            displayWorld.InitializeTilesCollection();
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
