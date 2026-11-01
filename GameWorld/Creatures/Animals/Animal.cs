using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Environment;
using Virtual_world.GameWorld.Graphics;

namespace Virtual_world.GameWorld.Creatures.Animals
{
    internal abstract class Animal : Organism
    {
        private readonly int _adultAgeThreshold;

        protected Animal(int X, int Y) : base(X, Y)
        {
            _adultAgeThreshold = 15 + World.Singleton.rngGen.Next(-2, 4);
        }

        public override string GraphicSrc => ImageManager.AnimalImages[this.Id];
        public virtual bool IsCarnivore => false;
        public bool IsAdult => Age > _adultAgeThreshold;

        public override void Action()
        {
            base.Action();
            for (int move = 0; move < Math.Round(Speed, MidpointRounding.AwayFromZero) && IsAlive; move++)
            {
                Animal? partner = null;
                foreach (var tile in GetAdjacentTiles())
                {
                    if (tile.Content is Animal adjacentAnimal
                        && adjacentAnimal.GetType() == GetType()
                        && IsAdult
                        && adjacentAnimal.IsAdult)
                    {
                        partner = adjacentAnimal;
                        break;
                    }
                }

                if (partner != null && TryReproduce(World.Singleton, partner))
                    continue;

                var targetTile = GetRandomAdjacentTile();
                if (targetTile != null)
                    TryMoveTo(targetTile);
            }
        }

        public override void Collision(World world, Organism other)
        {
            if (other == null || !IsAlive || !other.IsAlive || ReferenceEquals(this, other))
                return;

            if (other is Animal otherAnimal)
            {
                if (other.GetType() == GetType())
                    TryReproduce(world, otherAnimal);
                else
                    ResolveFight(world, otherAnimal);
                return;
            }

            if (other is Plant plant)
                plant.Collision(world, this);
        }

        internal void EatPlant(World world, Plant plant, double healthChange)
        {
            if (!IsAlive || !plant.IsAlive)
                return;

            var plantX = plant.X;
            var plantY = plant.Y;
            plant.MarkDead(world);
            world.TileSet[X, Y].Content = null;
            world.TileSet[plantX, plantY].Content = this;
            _x = plantX;
            _y = plantY;
            ChangeHealth(healthChange);

            if (Health <= 0)
                MarkDead(world);

            world.AddNarratorMessage($"{Name} zjadł {plant.Name}.");
        }

        private bool TryReproduce(World world, Animal partner)
        {
            if (!IsAlive || !partner.IsAlive || !IsAdult || !partner.IsAdult
                || world.rngGen.NextDouble() * 5 > ReproductionChance)
                return false;

            var childTile = GetRandomFreeAdjacentTile();
            if (childTile == null)
                return false;

            var child = Activator.CreateInstance(GetType(), childTile.X, childTile.Y);
            if (child is not Organism newborn)
                return false;

            newborn.Mutation(this, world);
            childTile.Content = newborn;
            world.AddNarratorMessage($"{Name} i {partner.Name} wydali na świat potomka.");
            return true;
        }

        private void ResolveFight(World world, Animal enemy)
        {
            var attacker = this;
            var defender = enemy;

            while (attacker.Health > 0 && defender.Health > 0)
            {
                if (attacker.Agility * 8 < world.rngGen.Next(100))
                {
                    defender.ChangeHealth(-attacker.Strength);
                }

                if (defender.Health <= 0)
                    break;

                if (defender.Agility * 10 < world.rngGen.Next(100))
                {
                    attacker.ChangeHealth(-defender.Strength);
                }
            }

            if (defender.Health <= 0)
            {
                defender.MarkDead(world);
                world.TileSet[defender.X, defender.Y].Content = attacker;
                world.TileSet[attacker.X, attacker.Y].Content = null;
                attacker._x = defender.X;
                attacker._y = defender.Y;
                if (attacker.IsCarnivore)
                    attacker.ChangeHealth(5);
                world.AddNarratorMessage($"{attacker.Name} pokonał {defender.Name}.");
                return;
            }

            if (attacker.Health <= 0)
            {
                attacker.MarkDead(world);
                world.TileSet[attacker.X, attacker.Y].Content = defender;
                world.TileSet[defender.X, defender.Y].Content = null;
                defender._x = attacker.X;
                defender._y = attacker.Y;
                if (defender.IsCarnivore)
                    defender.ChangeHealth(5);
                world.AddNarratorMessage($"{defender.Name} pokonał {attacker.Name}.");
                return;
            }
        }

        private void TryMoveTo(Tile target)
        {
            if (target == null)
                return;

            if (target.isEmpty)
            {
                MoveTo(target);
                return;
            }

            if (target.Content is Animal otherAnimal && otherAnimal.GetType() == GetType())
                return;

            if (target.Content != null)
            {
                Collision(World.Singleton, target.Content);
            }
        }
    }
}
