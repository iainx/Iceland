using System;
using GameplayKit;

using Iceland.Extensions;

namespace Iceland.Characters
{
    public class TalkableComponent : GKComponent, IMenuAction
    {
        public string Label { 
            get { return "Talk to"; } 
        }

        public TalkableComponent ()
        {
        }

        public void Activate (CharacterEntity playerEntity)
        {
            CharacterSpriteComponent spriteComp = playerEntity.GetComponent<CharacterSpriteComponent> ();
            spriteComp.LookAt ((CharacterEntity)Entity);

            Console.WriteLine ("Talking to {0}", ((CharacterEntity)Entity).Description);
        }
    }
}

