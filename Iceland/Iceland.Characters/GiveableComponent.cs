using System;

using GameplayKit;

using Iceland.Extensions;

namespace Iceland.Characters
{
    public class GiveableComponent : GKComponent, IMenuAction
    {
        public string Label {
            get {
                return "Give";
            }
        }

        public int NumberOfEntitiesNeeded {
            get {
                return 2;
            }
        }

        public bool OnlyIfCollected {
            get {
                return true;
            }
        }

        public bool OnlyIfDropped {
            get {
                return false;
            }
        }

        public void Activate (Entity playerEntity, Entity otherEntity)
        {
            var entity = (Entity)Entity;
            var model = entity.Model;

            CharacterSpriteComponent spriteComp = playerEntity.GetComponent<CharacterSpriteComponent> ();
            spriteComp.LookAt (otherEntity);

            GiveHandler.StartGive (playerEntity, entity, otherEntity);
        }
    }
}

