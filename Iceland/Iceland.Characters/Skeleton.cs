using System;
using GameplayKit;

namespace Iceland.Characters
{
    public class Skeleton : CharacterEntity
    {
        public Skeleton ()
        {
            AddComponent (new CharacterSpriteComponent ("skeleton_walk_cycle", this));
            AddComponent (new ClickableComponent ());
        }
    }
}

