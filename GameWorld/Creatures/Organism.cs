using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Creatures
{
    internal abstract class Organism
    {
        protected Organism(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
            Age = 0;
        }
        public int Strength;
        public int Initiative;
        public int X;
        public int Y;
        public int Health;
        public string Name = "";
        public int Age;
        public int Dexterity;
        public int Id;
        public double ReproductionChance;
        public abstract string GraphicSrc { get; }
        public double SpawnRate;

        public virtual void Action(World world)
        {
            Age++;
        }
        public abstract void Collision(World world, Organism other);
        public abstract void Mutation();

        public virtual List<Organism?> GetAdjacentTiles()
        {
            List<Organism?> result = new List<Organism?>();
            for (int x = this.X - 1; x <= this.X + 1; x++)
            {
                for (int y = this.Y - 1; y <= this.Y + 1; y++)
                {
                    result.Add(World.WorldSingleton.TileSet[x, y]);
                }
            }
            return result;
        }
    }
}
