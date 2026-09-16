using System;
using System.Collections.Generic;
using System.Text;

namespace Virtual_world.GameWorld.Creatures
{
    internal abstract class Organism
    {
        public int Strength { get; set; }
        public int Initiative { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Health { get; set; }
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public int Dexterity { get; set; }
        public int Id { get; set; }
        public double ReproductionChance { get; set; }
        public string GraphicSrc { get; set; } = "";
        public double SpawnRate { get; set; }

        public abstract void Action();
        public abstract void Collision(Organism other);
        public abstract void Mutation();
    }
}
