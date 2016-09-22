using System;

using Foundation;
using GameplayKit;

using Iceland.Extensions;

namespace Iceland.Characters
{
    public class CollectibleComponent : GKComponent, IMenuAction
    {
        public string Label {
            get {
                return "Pick up";
            }
        }

        public bool OnlyIfCollected {
            get {
                return false;
            }
        }

        public bool OnlyIfDropped {
            get {
                return true;
            }
        }

        public void Activate (Entity playerEntity, Entity otherEntity)
        {
            Entity entity = (Entity)Entity;
            Console.WriteLine ($"Picking up {entity.Name}");

            CharacterSpriteComponent spriteComp = playerEntity.GetComponent<CharacterSpriteComponent> ();
            spriteComp.LookAt (entity);

            CollectHandler.StartCollect (playerEntity, entity);
        }
    }
}

