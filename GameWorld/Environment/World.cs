using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Creatures;
using Virtual_world.GameWorld.Graphics;

namespace Virtual_world.GameWorld.Environment
{
    public class World
    {
        public World(int X, int Y)
        {
            SizeX = X;
            SizeY = Y;
            TileSet = new Organism?[SizeX, SizeY];
            Singleton = this;
            DisplayWorld.Singleton.UpdateTilesCollection();
        }
        public void DoTurn() { }
        public void DrawWorld() { }

        public readonly int SizeX;
        public readonly int SizeY;

        public static World Singleton = new World(20, 20);

        public Organism?[,] TileSet;
    }
}
