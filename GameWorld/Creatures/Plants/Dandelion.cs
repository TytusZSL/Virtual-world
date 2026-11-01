using System;
using Virtual_world.GameWorld.Creatures.Animals;
using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Creatures.Plants
{
    internal class Dandelion : Plant
    {
        public const double SpawnRate = 0.5;

        private readonly int _initiative = 0;
        private readonly int _dexterity = 0;

        public Dandelion(int X, int Y) : base(X, Y)
        {
            Health = 1;
            Strength = 0;
            ReproductionChance = 0.0;
        }

        public override double Strength { get; protected set; }
        public override int Initiative => _initiative;
        public override double Health { get; protected set; }
        public override string Name => "mlecz";
        public override int Agility => _dexterity;
        public override int Id => 1;
        public override double ReproductionChance { get; protected set; }
        public override string GraphicSrc => "mlecz.png";

        public override void Action()
        {
            base.Action();
            if (!IsAlive)
                return;

            for (int i = 0; i < 3; i++)
            {
                var target = GetRandomFreeAdjacentTile();
                if (target == null)
                    break;

                if (World.Singleton.rngGen.NextDouble() > 0.15)
                {
                    var plant = Activator.CreateInstance(GetType(), target.X, target.Y);
                    if (plant is Organism newPlant)
                    {
                        newPlant.Mutation(this, World.Singleton);
                        World.Singleton.TileSet[target.X, target.Y].Content = newPlant;
                    }
                }
            }
        }

        public override void Collision(World world, Organism other)
        {
            if (other is Animal animal)
                ConsumeByAnimal(world, animal, 1);
        }
    }
}
