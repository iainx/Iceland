using System;

namespace Iceland.Characters
{
    public class Professor : CharacterEntity
    {
        public Professor ()
        {
            AddComponent (new CharacterSpriteComponent ("professor_walk_cycle_no_hat", this));
        }
    }
}

