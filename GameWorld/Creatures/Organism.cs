using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Creatures
{
    public abstract class Organism
    {
        public Organism(int X, int Y)
        {
            _x = X;
            _y = Y;
        }
        public abstract int Strength { get; }
        public abstract int Initiative { get; }
        public int X => _x;
        protected int _x;
        public int Y => _y;
        protected int _y;
        public abstract int Health { get; }
        public abstract string Name { get; }
        public int Age => _age;
        private int _age = 0;
        public abstract int Dexterity { get; }
        public abstract int Id { get; }
        public abstract double ReproductionChance { get; }
        public abstract string GraphicSrc { get; }
        public double SpawnRate;

        public Tile currentTile => World.Singleton.TileSet[X, Y];

        public virtual void Action()
        {
            _age++;
        }
        public abstract void Collision(World world, Organism other);
        public abstract void Mutation();

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
    }
}
