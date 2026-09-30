using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Environment;
using Virtual_world.GameWorld.Graphics;

namespace Virtual_world.GameWorld.Creatures.Animals
{
    internal abstract class Animal : Organism
    {
        protected Animal(int X, int Y) : base(X, Y)
        {
            
        }

        public override string GraphicSrc => ImageManager.AnimalImages[this.Id];

        public override void Action()
        {
            base.Action();
            List<Tile> adjacentTiles = GetAdjacentTiles(); // here we assume that there are always adjacent tiles
            Tile targetTile = adjacentTiles[World.Singleton.rngGen.Next(adjacentTiles.Count)];
            TryMoveTo(targetTile);
        }

        private void TryMoveTo(Tile target)
        {
            if (target.isEmpty)
            {
                target.Content = currentTile.Content;
                currentTile.Content = null;
                _x = target.X;
                _y = target.Y;
            }
        }
    }
}
