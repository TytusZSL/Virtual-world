using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Graphics;

namespace Virtual_world.GameWorld.Creatures
{
    internal abstract class Plant : Organism
    {
        protected Plant(int X, int Y) : base(X, Y)
        {

        }
        public override string GraphicSrc => ImageManager.PlantImages[this.Id];
    }
}
