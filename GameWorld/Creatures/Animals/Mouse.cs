using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Creatures.Animals
{
    internal class Mouse : Animal
    {
        public const double SpawnRate = 0.5;

        public Mouse(int X, int Y) : base(X, Y)
        {
            Health = 12;
            Strength = 1;
            ReproductionChance = 0.5;
        }

        public override double Strength { get; protected set; }
        public override int Initiative { get; protected set; } = 6;
        public override double Health { get; protected set; }
        public override string Name { get; protected set; } = "mysz";
        public override int Agility { get; protected set; } = 12;
        public override int Id => 10;
        public override double ReproductionChance { get; protected set; }
        public override string GraphicSrc => "mysz.png";
        public override double MutationChanceMultiplier => 3.0;
        public override void Collision(World world, Organism other) => base.Collision(world, other);
    }
}
