using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Creatures.Animals;
using Virtual_world.GameWorld.Environment;
using Virtual_world.GameWorld.Graphics;

namespace Virtual_world.GameWorld.Creatures
{
    internal abstract class Plant : Organism
    {
        protected Plant(int X, int Y) : base(X, Y)
        {
        }

        public override string GraphicSrc => ImageManager.PlantImages[this.Id];

        public override void Action()
        {
            base.Action();
            if (!IsAlive)
                return;

            var targetTile = GetRandomFreeAdjacentTile();
            if (targetTile == null || World.Singleton.rngGen.NextDouble() > 0.15)
                return;

            var plant = Activator.CreateInstance(GetType(), targetTile.X, targetTile.Y);
            if (plant is Organism newPlant)
            {
                newPlant.Mutation(this, World.Singleton);
                World.Singleton.TileSet[targetTile.X, targetTile.Y].Content = newPlant;
            }
        }

        public override void Collision(World world, Organism other)
        {
            if (other is Animal)
            {
                MarkDead(world);
                world.AddNarratorMessage($"{other.Name} zjadł {Name}.");
            }
        }
    }
}
