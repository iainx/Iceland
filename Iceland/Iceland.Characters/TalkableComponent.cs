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
        public static readonly string CharacterEntityKey = "TalkableComponent.CharacterEntityKey";
        public static readonly string PlayerEntityKey = "TalkableComponent.PlayerEntityKey";
        public static readonly string ConversationKey = "TalkableComponent.ConversationKey";
        public static readonly string RunConversationNotification = "TalkableComponent.RunConversationNotification";

        public string Label { 
            get { return "Talk to"; } 
        }

        public int CurrentConversation { get; set; }
        public List<Details> Conversations { get; private set; }

        public TalkableComponent ()
        {
            CurrentConversation = 0;
            Conversations = new List<Details> ();
        }

        public void Activate (CharacterEntity playerEntity)
        {
            if (Conversations.Count == 0) {
                return;
            }

            if (CurrentConversation == -1) {
                return;
            }

            CharacterSpriteComponent spriteComp = playerEntity.GetComponent<CharacterSpriteComponent> ();
            spriteComp.LookAt ((CharacterEntity)Entity);

            NSDictionary userInfo = new NSDictionary ((NSString)CharacterEntityKey, Entity, 
                                                      (NSString)PlayerEntityKey, playerEntity,
                                                      ConversationKey, NSObject.FromObject (Conversations [CurrentConversation]));
            var nc = NSNotificationCenter.DefaultCenter;

            nc.PostNotificationName (RunConversationNotification, Entity, userInfo);
        }
    }
}

