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

            var conversation = model.CurrentConversation;

            if (conversation == null) {
                return;
            }

            CharacterSpriteComponent spriteComp = playerEntity.GetComponent<CharacterSpriteComponent> ();
            spriteComp.LookAt ((Entity)Entity);

            ConversationHandler.StartConversation (playerEntity, (Entity)Entity, conversation.Item1, conversation.Item2);
        }
    }
}

