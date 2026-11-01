using System;
using System.Collections.Generic;
using System.Text;

namespace Virtual_world.GameWorld.Graphics
{
    public class NarratorEntry
    {
        public NarratorEntry(string entryText, EntryType type)
        {
            EntryText = entryText;
            Type = type;
        }
        public string EntryText { get; init; }
        public EntryType Type { get; init; }
    }

    public enum EntryType
    {
        Death,
        Fight,
        Consume,
        Breed,
        World
    }
}
