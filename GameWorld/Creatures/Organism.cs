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
        public abstract int Initiative { get; protected set; }
        public int X => _x;
        protected int _x;
        public int Y => _y;
        protected int _y;
        public abstract double Health { get; protected set; }
        public abstract string Name { get; protected set; }
        public int Age => _age;
        private int _age = 0;
        public bool IsAlive { get; private set; } = true;
        public abstract int Agility { get; protected set; }
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

        public void UpdateEditableAttributes(string name, int age, double health, double strength, int initiative, int agility, double speed, double reproductionChance)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nazwa organizmu nie może być pusta.", nameof(name));
            if (age < 0)
                throw new ArgumentOutOfRangeException(nameof(age), "Wiek nie może być ujemny.");
            if (!double.IsFinite(health) || health < 0)
                throw new ArgumentOutOfRangeException(nameof(health), "Zdrowie musi być liczbą nieujemną.");
            if (!double.IsFinite(strength) || strength < 0)
                throw new ArgumentOutOfRangeException(nameof(strength), "Siła musi być liczbą nieujemną.");
            if (initiative < 0)
                throw new ArgumentOutOfRangeException(nameof(initiative), "Inicjatywa nie może być ujemna.");
            if (agility < 0 || agility > 100)
                throw new ArgumentOutOfRangeException(nameof(agility), "Zwinność musi mieścić się w zakresie od 0 do 100.");
            if (!double.IsFinite(speed) || speed < 1 || speed > 100)
                throw new ArgumentOutOfRangeException(nameof(speed), "Prędkość musi mieścić się w zakresie od 1 do 100.");
            if (!double.IsFinite(reproductionChance) || reproductionChance < 0 || reproductionChance > 5)
                throw new ArgumentOutOfRangeException(nameof(reproductionChance), "Szansa na rozmnożenie musi mieścić się w zakresie od 0 do 5.");

            Name = name.Trim();
            _age = age;
            Health = health;
            Strength = strength;
            Initiative = initiative;
            Agility = agility;
            Speed = speed;
            ReproductionChance = reproductionChance;

            if (Health == 0 || Age > 100)
                MarkDead(World.Singleton);
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
