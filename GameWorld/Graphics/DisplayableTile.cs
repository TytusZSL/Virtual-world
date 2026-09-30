using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Creatures;
using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Graphics
{
    public class DisplayableTile
    {
        public DisplayableTile(Tile Tile)
        {
            this.Tile = Tile;
        }
        private Tile Tile;

        public string GraphicSrc => Tile.Content?.GraphicSrc ?? "none.png";
    }
}
