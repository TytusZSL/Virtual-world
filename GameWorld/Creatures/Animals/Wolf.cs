using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Creatures.Animals
{
    internal class Wolf : Animal
    {
        public override int Strength => _strength;
        private int _strength = 9;
        public override int Initiative => _initiative;
        private int _initiative = 5;
        public override int Health => _health;
        private int _health;
        public override string Name => _name;
        private string _name = "wilk";
        public override int Dexterity => _dexterity;
        private int _dexterity;
        public override int Id => 1;
        public override double ReproductionChance => _reproductionChance;
        private double _reproductionChance;

        public override string GraphicSrc => "Wolf.png";
        public double SpawnRate;
        public Wolf(int X, int Y) : base(X, Y)
        {
        }

        public override void Action()
        {
            base.Action();
            Console.WriteLine($"{X}, {Y}");
        }

        public override void Collision(World world, Organism other)
        {
            
        }

        public override void Mutation()
        {
            
        }
    }
}
