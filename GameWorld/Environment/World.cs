using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Creatures;

namespace Virtual_world.GameWorld.Environment
{
    internal class World
    {
        public World(int X, int Y)
        {
            SizeX = X;
            SizeY = Y;
            TileSet = new Organism?[SizeX, SizeY];
            WorldSingleton = this;
        }
        public void DoTurn() { }
        public void DrawWorld() { }

        public readonly int SizeX;
        public readonly int SizeY;

        public static World WorldSingleton = new World(20, 20);

        public Organism?[,] TileSet;
    }
}
