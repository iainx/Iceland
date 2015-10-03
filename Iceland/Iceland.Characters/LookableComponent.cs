using System;

using GameplayKit;

namespace Iceland.Characters
{
    public class LookableComponent : GKComponent, IMenuAction
    {
        public string Label { 
            get { return "Look at"; } 
        }

        public LookableComponent ()
        {
        }

        public void Activate (CharacterEntity playerEntity)
        {
            Console.WriteLine ("Looking at {0}", ((CharacterEntity)Entity).Description);
        }
    }
}

