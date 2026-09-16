using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Environment;
using Virtual_world.GameWorld.Graphics;

namespace Virtual_world.GameWorld.Creatures
{
    internal abstract class Animal : Organism
    {
        protected Animal(int X, int Y) : base(X, Y)
        {
            
        }

        public override string GraphicSrc => ImageManager.AnimalImages[this.Id];

        public override void Action(World world)
        {
            base.Action(world);
            List<Organism?> adjacentTiles = GetAdjacentTiles(world);
        }
    }
}
