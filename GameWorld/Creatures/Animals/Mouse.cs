using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Creatures.Animals
{
    internal class Mouse : Animal
    {
        public const double SpawnRate = 0.5;

        private readonly int _initiative = 6;
        private readonly int _dexterity = 0;

        public Mouse(int X, int Y) : base(X, Y)
        {
            Health = 12;
            Strength = 1;
            ReproductionChance = 0.5;
        }

        public override double Strength { get; protected set; }
        public override int Initiative => _initiative;
        public override double Health { get; protected set; }
        public override string Name => "mysz";
        public override int Agility => _dexterity;
        public override int Id => 10;
        public override double ReproductionChance { get; protected set; }
        public override string GraphicSrc => "mysz.png";
        public override double MutationChanceMultiplier => 3.0;
        public override void Collision(World world, Organism other) => base.Collision(world, other);
    }
}
