using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Creatures.Animals
{
    internal class Fox : Animal
    {
        public const double SpawnRate = 0.2;

        private readonly int _initiative = 7;
        private readonly int _dexterity = 0;

        public Fox(int X, int Y) : base(X, Y)
        {
            Health = 27;
            Strength = 3;
            ReproductionChance = 0.5;
        }

        public override double Strength { get; protected set; }
        public override int Initiative => _initiative;
        public override double Health { get; protected set; }
        public override bool IsCarnivore => true;
        public override string Name => "lis";
        public override int Agility => _dexterity;
        public override int Id => 3;
        public override double ReproductionChance { get; protected set; }
        public override string GraphicSrc => "lis.png";
        public override void Collision(World world, Organism other) => base.Collision(world, other);
    }
}
