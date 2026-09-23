using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Creatures;

namespace Virtual_world.GameWorld.Graphics
{
    public class DisplayableTile
    {
        public DisplayableTile(Organism? organism)
        {
            this.organism = organism;
        }
        private Organism? organism;

        public string GraphicSrc => organism?.GraphicSrc ?? "none.png";
    }
}
