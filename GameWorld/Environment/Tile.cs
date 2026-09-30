using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Creatures;

namespace Virtual_world.GameWorld.Environment
{
    public class Tile
    {
        public Tile(int X, int Y)
        {
            _x = X;
            _y = Y;
        }
        public Organism? Content;
        public int X => _x;
        private int _x;
        public int Y => _y;
        private int _y;

        public bool isEmpty => Content == null;
    }
}
