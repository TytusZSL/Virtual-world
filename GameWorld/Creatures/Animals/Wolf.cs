using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Creatures.Animals
{
    internal class Wolf : Animal
    {
        public const double SpawnRate = 0.1;

        public override double Strength { get; protected set; }
        public override int Initiative { get; protected set; } = 5;
        public override double Health { get; protected set; }
        public override bool IsCarnivore => true;
        public override string Name { get; protected set; } = "wilk";
        public override int Agility { get; protected set; } = 8;
        public override int Id => 1;
        public override double ReproductionChance { get; protected set; }
        public override string GraphicSrc => "wilk.png";

        public Wolf(int X, int Y) : base(X, Y)
        {
            Health = 36;
            Strength = 9;
            ReproductionChance = 0.5;
        }
        public override void Collision(World world, Organism other) => base.Collision(world, other);
    }
}
