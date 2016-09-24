using System;
using System.Collections.Generic;
using GameplayKit;

using Iceland.Extensions;
using Iceland.Conversation;

using Foundation;

namespace Iceland.Characters
{
    public class TalkableComponent : GKComponent, IMenuAction
    {
        public string Label { 
            get { return "Talk to"; } 
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

