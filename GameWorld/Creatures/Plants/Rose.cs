using Virtual_world.GameWorld.Creatures.Animals;
using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Creatures.Plants
{
    internal class Rose : Plant
    {
        public const double SpawnRate = 0.3;

        public Rose(int X, int Y) : base(X, Y)
        {
            Health = 30;
            Strength = 0;
            ReproductionChance = 0.0;
        }

        public override double Strength { get; protected set; }
        public override int Initiative { get; protected set; }
        public override double Health { get; protected set; }
        public override string Name { get; protected set; } = "róża";
        public override int Agility { get; protected set; }
        public override int Id => 3;
        public override double ReproductionChance { get; protected set; }
        public override string GraphicSrc => "roza.png";

        public override void Collision(World world, Organism other)
        {
            if (other is Animal animal && animal.IsAlive && IsAlive)
            {
                ConsumeByAnimal(world, animal, -1);
                if (animal.IsAlive)
                    animal.IncreaseStrengthByOne();
            }
        }
    }
}
