using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Creatures.Animals
{
    internal class Sheep : Animal
    {
        public const double SpawnRate = 0.3;

        public Sheep(int X, int Y) : base(X, Y)
        {
            Health = 24;
            Strength = 4;
            ReproductionChance = 0.5;
        }

        public override double Strength { get; protected set; }
        public override int Initiative { get; protected set; } = 4;
        public override double Health { get; protected set; }
        public override string Name { get; protected set; } = "owca";
        public override int Agility { get; protected set; } = 5;
        public override int Id => 2;
        public override double ReproductionChance { get; protected set; }
        public override string GraphicSrc => "owca.png";
        public override void Collision(World world, Organism other) => base.Collision(world, other);
    }
}
