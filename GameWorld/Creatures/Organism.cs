using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Creatures
{
    public abstract class Organism
    {
        private readonly int _lifespanVariation;

        public Organism(int X, int Y)
        {
            _x = X;
            _y = Y;
            _lifespanVariation = World.Singleton.rngGen.Next(-5, 6);
        }
        public abstract double Strength { get; protected set; }
        public abstract int Initiative { get; }
        public int X => _x;
        protected int _x;
        public int Y => _y;
        protected int _y;
        public abstract double Health { get; protected set; }
        public abstract string Name { get; }
        public int Age => _age;
        private int _age = 0;
        public bool IsAlive { get; private set; } = true;
        public abstract int Agility { get; }
        public abstract int Id { get; }
        public abstract double ReproductionChance { get; protected set; }
        public double Speed { get; protected set; } = 1.0;
        public virtual double MutationChanceMultiplier => 1.0;
        public abstract string GraphicSrc { get; }

        public Tile currentTile => World.Singleton.TileSet[X, Y];

        public virtual void Action()
        {
            if (!IsAlive)
                return;

            _age++;
            if (_age > 100 || _age > Health + _lifespanVariation)
            {
                MarkDead(World.Singleton);
                World.Singleton.AddNarratorMessage($"{Name} zmarł ze starości.", Graphics.EntryType.Death);
            }
        }

        internal void MarkDead(World world)
        {
            if (!IsAlive)
                return;

            IsAlive = false;
            if (X >= 0 && X < world.Size && Y >= 0 && Y < world.Size && world.TileSet[X, Y].Content == this)
                world.TileSet[X, Y].Content = null;
        }

        public void ChangeHealth(double amount)
        {
            Health = Math.Max(0, Health + amount);
        }

        public abstract void Collision(World world, Organism other);

        public virtual void Mutation(Organism parent, World world)
        {
            Health = parent.Health;
            ReproductionChance = parent.ReproductionChance;
            Strength = parent.Strength;
            Speed = parent.Speed;

            var mutationChance = Math.Min(1.0, world.MutationChance * MutationChanceMultiplier);
            if (world.rngGen.NextDouble() >= mutationChance)
                return;

            Health = parent.Health + (world.rngGen.NextDouble() * 2 - 1);
            if (Health <= 0)
                Health = 1;

            ReproductionChance = Math.Max(0.0, parent.ReproductionChance + (world.rngGen.NextDouble() - 0.5) / 15);

            if (parent.Strength > 0)
            {
                Strength = Math.Max(0.0, parent.Strength + (world.rngGen.NextDouble() - 0.5) / 20);
            }

            Speed = Math.Max(1.0, parent.Speed + (world.rngGen.NextDouble() - 0.5) / 20);
        }

        public virtual List<Tile> GetAdjacentTiles()
        {
            List<Tile> result = new List<Tile>();
            for (int x = this.X - 1; x <= this.X + 1; x++)
            {
                for (int y = this.Y - 1; y <= this.Y + 1; y++)
                {
                    if (x >= 0 && x < World.Singleton.Size && y >= 0 && y < World.Singleton.Size && (x != this.X || y != this.Y))
                        result.Add(World.Singleton.TileSet[x, y]);
                }
            }
            return result;
        }

        protected Tile? GetRandomAdjacentTile()
        {
            var tiles = GetAdjacentTiles();
            if (tiles.Count == 0)
                return null;

            return tiles[World.Singleton.rngGen.Next(tiles.Count)];
        }

        protected Tile? GetRandomFreeAdjacentTile()
        {
            var freeTiles = GetAdjacentTiles().Where(t => t.isEmpty).ToList();
            if (freeTiles.Count == 0)
                return null;

            return freeTiles[World.Singleton.rngGen.Next(freeTiles.Count)];
        }

        protected void MoveTo(Tile target)
        {
            if (target == null || !target.isEmpty)
                return;

            currentTile.Content = null;
            target.Content = this;
            _x = target.X;
            _y = target.Y;
        }
    }
}
