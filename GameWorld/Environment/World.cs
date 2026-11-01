using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Creatures;
using Virtual_world.GameWorld.Creatures.Animals;
using Virtual_world.GameWorld.Creatures.Plants;
using Virtual_world.GameWorld.Graphics;

namespace Virtual_world.GameWorld.Environment
{
    public class World
    {
        public World(int size, double mutationChance) : this(size, mutationChance, DisplayWorld.Singleton)
        {
        }

        public World(int size, double mutationChance, DisplayWorld displayWorld)
        {
            Size = size;
            MutationChance = mutationChance;
            TileSet = new Tile[size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    TileSet[x, y] = new Tile(x, y);
                }
            }

            Singleton = this;
            SpawnInitialPopulation();
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

        public List<string> NarratorMessages { get; } = new();

        public void AddNarratorMessage(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                NarratorMessages.Add(message);
            }
        }

        public void SpawnInitialPopulation()
        {
            var species = new (Func<int, int, Organism> Create, double SpawnRate)[]
            {
                ((x, y) => new Wolf(x, y), Wolf.SpawnRate),
                ((x, y) => new Sheep(x, y), Sheep.SpawnRate),
                ((x, y) => new Fox(x, y), Fox.SpawnRate),
                ((x, y) => new Mouse(x, y), Mouse.SpawnRate),
                ((x, y) => new Grass(x, y), Grass.SpawnRate),
                ((x, y) => new Dandelion(x, y), Dandelion.SpawnRate),
                ((x, y) => new Thorn(x, y), Thorn.SpawnRate)
            };
            var rowColumnFactor = Math.Floor(Math.Sqrt(TileSet.Length));
            var populations = species
                .Select(s => (s.Create, Count: (int)Math.Floor(rowColumnFactor * s.SpawnRate)))
                .ToList();
            var totalPopulation = populations.Sum(p => p.Count);
            if (totalPopulation > TileSet.Length)
                throw new InvalidOperationException("The configured spawn rates exceed the world's available tiles.");

            foreach (var tile in TileSet)
            {
                tile.Content = null;
            }

            NarratorMessages.Clear();
            var freeTiles = TileSet.Cast<Tile>().ToList();
            foreach (var population in populations)
            {
                for (int i = 0; i < population.Count; i++)
                {
                    var tileIndex = rngGen.Next(freeTiles.Count);
                    var tile = freeTiles[tileIndex];
                    freeTiles.RemoveAt(tileIndex);
                    tile.Content = population.Create(tile.X, tile.Y);
                }
            }

            AddNarratorMessage("Początek świata: wygenerowano organizmy na losowych polach.");
        }

        public bool AddOrganism(Organism organism)
        {
            if (organism == null)
                return false;

            if (organism.X < 0 || organism.X >= Size || organism.Y < 0 || organism.Y >= Size)
                return false;

            if (TileSet[organism.X, organism.Y].Content != null)
                return false;

            TileSet[organism.X, organism.Y].Content = organism;
            return true;
        }

        private double _mutationChance = 0.1;

        public double MutationChance
        {
            get => _mutationChance;
            set
            {
                if (double.IsNaN(value) || value < 0.0 || value > 1.0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Mutation chance must be between 0 and 1.");

                _mutationChance = value;
            }
        }

        public readonly int Size;

        public static World Singleton = new World(20, 0.1);

        public Tile[,] TileSet;

        public Random rngGen = new Random();
    }
}
