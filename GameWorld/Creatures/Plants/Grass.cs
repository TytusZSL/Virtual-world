using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Creatures.Plants
{
    internal class Grass : Plant
    {
        public const double SpawnRate = 0.8;

        private readonly int _initiative = 0;
        private readonly int _dexterity = 0;

        public Grass(int X, int Y) : base(X, Y)
        {
            Health = 12;
            Strength = 0;
            ReproductionChance = 0.0;
        }

        public override double Strength { get; protected set; }
        public override int Initiative => _initiative;
        public override double Health { get; protected set; }
        public override string Name => "trawa";
        public override int Agility => _dexterity;
        public override int Id => 0;
        public override double ReproductionChance { get; protected set; }
        public override string GraphicSrc => "trawa.png";
    }
}
