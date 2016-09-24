using System;

using Foundation;
using GameplayKit;

using Iceland.Extensions;

namespace Iceland.Characters
{
    public class LookableComponent : GKComponent, IMenuAction
    {
        public string Label { 
            get { return "Look at"; } 
        }

        public int NumberOfEntitiesNeeded {
            get {
                return 1;
            }
        }

        public bool OnlyIfCollected {
            get {
                return false;
            }
        }

        public bool OnlyIfDropped {
            get {
                return false;
            }
        }

        public void Activate (Entity playerEntity, Entity otherEntity)
        {
            CharacterSpriteComponent spriteComp = playerEntity.GetComponent<CharacterSpriteComponent> ();
            spriteComp.LookAt ((Entity)Entity);

            LookHandler.StartLook (playerEntity, (Entity)Entity);
        }
    }
}

