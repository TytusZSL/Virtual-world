using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Creatures.Animals
{
    internal class Fox : Animal
    {
        public const double SpawnRate = 0.2;

        public Fox(int X, int Y) : base(X, Y)
        {
            Health = 27;
            Strength = 3;
            ReproductionChance = 0.5;
        }

        public override double Strength { get; protected set; }
        public override int Initiative { get; protected set; } = 7;
        public override double Health { get; protected set; }
        public override bool IsCarnivore => true;
        public override string Name { get; protected set; } = "lis";
        public override int Agility { get; protected set; }
        public override int Id => 3;
        public override double ReproductionChance { get; protected set; }
        public override string GraphicSrc => "lis.png";
        public override void Collision(World world, Organism other) => base.Collision(world, other);
    }
}
