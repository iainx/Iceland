using System;
using GameplayKit;

using Iceland.Map;

namespace Iceland.Characters
{
    public class CharacterEntity : GKEntity
    {
        public Map.Map.Position CurrentPosition { get; set; }
        public Map.Map Map { get; set; }
    }
}

