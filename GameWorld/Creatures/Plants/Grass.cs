using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Creatures.Plants
{
    internal class Grass : Plant
    {
        public const double SpawnRate = 0.8;

        public Grass(int X, int Y) : base(X, Y)
        {
            Health = 12;
            Strength = 0;
            ReproductionChance = 0.0;
        }

        public override double Strength { get; protected set; }
        public override int Initiative { get; protected set; }
        public override double Health { get; protected set; }
        public override string Name { get; protected set; } = "trawa";
        public override int Agility { get; protected set; }
        public override int Id => 0;
        public override double ReproductionChance { get; protected set; }
        public override string GraphicSrc => "trawa.png";
    }
}
