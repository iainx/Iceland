using System;

using GameplayKit;

using Iceland.Extensions;

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
            CharacterSpriteComponent spriteComp = playerEntity.GetComponent<CharacterSpriteComponent> ();
            spriteComp.LookAt ((CharacterEntity)Entity);

            Console.WriteLine ("Looking at {0}", ((CharacterEntity)Entity).Description);
        }
    }
}

