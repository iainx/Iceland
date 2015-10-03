using System;
using GameplayKit;

namespace Iceland.Characters
{
    public class Skeleton : CharacterEntity
    {
        public Skeleton ()
        {
            Name = "Death";
            Description = "A cold looking skeleton";
            AddComponent (new CharacterSpriteComponent ("skeleton_walk_cycle", this));
            AddComponent (new ClickableComponent ());
            AddComponent (new TalkableComponent ());
            AddComponent (new LookableComponent ());
        }
    }
}

