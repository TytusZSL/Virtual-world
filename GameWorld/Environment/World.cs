using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Creatures;

namespace Virtual_world.GameWorld.Environment
{
    internal class World
    {
        public void DoTurn() { }
        public void DrawWorld() { }

        Organism[,] TileSet = new Organism[20, 20];
    }
}
