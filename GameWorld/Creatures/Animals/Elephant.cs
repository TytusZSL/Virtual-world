using Virtual_world.GameWorld.Environment;
using Virtual_world.GameWorld.Graphics;

namespace Virtual_world.GameWorld.Creatures.Animals
{
    internal class Elephant : Animal
    {
        public const double SpawnRate = 0.1;

        public Elephant(int X, int Y) : base(X, Y)
        {
            Health = 100;
            Strength = 10;
            ReproductionChance = 0.5;
        }

        public override double Strength { get; protected set; }
        public override int Initiative { get; protected set; } = 1;
        public override double Health { get; protected set; }
        public override string Name { get; protected set; } = "słoń";
        public override int Agility { get; protected set; } = 1;
        public override int Id => 5;
        public override double ReproductionChance { get; protected set; }
        public override string GraphicSrc => "slon.png";

        public override void Collision(World world, Organism other)
        {
            if (other is Animal animal && AutomaticallyDefeats(animal) && IsAlive && animal.IsAlive)
            {
                MoveToDefeatedAnimalTile(world, animal);
                world.AddNarratorMessage($"{Name} rozdeptał {animal.Name}.", EntryType.Fight);
                return;
            }

            base.Collision(world, other);
        }

        internal override bool AutomaticallyDefeats(Animal otherAnimal) =>
            otherAnimal.GetType() != GetType() && otherAnimal.Strength < 4;
    }
}
