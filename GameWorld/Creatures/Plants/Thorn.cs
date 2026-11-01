using System;
using Virtual_world.GameWorld.Creatures.Animals;
using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Creatures.Plants
{
    internal class Thorn : Plant
    {
        public const double SpawnRate = 0.2;

        public Thorn(int X, int Y) : base(X, Y)
        {
            Health = 2;
            Strength = 0;
            ReproductionChance = 0.0;
        }

        public override double Strength { get; protected set; }
        public override int Initiative { get; protected set; }
        public override double Health { get; protected set; }
        public override string Name { get; protected set; } = "cierń";
        public override int Agility { get; protected set; }
        public override int Id => 2;
        public override double ReproductionChance { get; protected set; }
        public override string GraphicSrc => "ciern.png";

        public override void Action()
        {
            base.Action();
            if (!IsAlive)
                return;

            var target = GetRandomFreeAdjacentTile();
            if (target == null)
                return;

            var plant = Activator.CreateInstance(GetType(), target.X, target.Y);
            if (plant is Organism newPlant)
            {
                newPlant.Mutation(this, World.Singleton);
                World.Singleton.TileSet[target.X, target.Y].Content = newPlant;
            }
        }

        public override void Collision(World world, Organism other)
        {
            if (other is Animal animal)
                ConsumeByAnimal(world, animal, -4);
        }
    }
}
