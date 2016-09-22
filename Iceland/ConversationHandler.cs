using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Foundation;
using UIKit;

using Iceland.Characters;
using Iceland.Conversation;

namespace Iceland
{
    public class ConversationHandler
    {
        public static readonly string CharacterEntityKey = "ConversationHandler.CharacterEntityKey";
        public static readonly string PlayerEntityKey = "ConversationHandler.PlayerEntityKey";
        public static readonly string ConversationKey = "ConversationHandler.ConversationKey";
        public static readonly string ConversationIdKey = "ConversationHandler.ConversationIDKey";
        public static readonly string RunConversationNotification = "ConversationHandler.RunConversationNotification";

        UIViewController controller;

        public ConversationHandler (UIViewController mainController)
        {
            var nc = NSNotificationCenter.DefaultCenter;
            nc.AddObserver ((NSString)RunConversationNotification, RunConversation);

            controller = mainController;
        }

        UIViewController CreateControllerForConversation (ConversationItem[] items, int currentItem, Entity speaker)
        {
            var item = items [currentItem];

            /*
            if (item.DependencyScripts != null) {
                foreach (var script in item.DependencyScripts) {
                    LuaEngine.ExecuteScript (script, item);
                }
            }
*/
            var text = string.Join ("\n", item.Character);
            var alert = UIAlertController.Create (speaker.Model.Name, text, UIAlertControllerStyle.Alert);

            // Don't want to evaluate the ids everytime
            var ids = item.ResponseIds;
            if (ids == null || ids.Length == 0) {
                Console.WriteLine ("No responses");
                return alert;
            }

            foreach (var responseId in ids) {
                var localResponseId = responseId - 1;
                var responseItem = items [localResponseId];

                var a = UIAlertAction.Create (responseItem.Player[0], 0, async (obj) =>  {
                    controller.DismissViewController (false, null);

                    if (responseItem.ActionScripts != null) {
                        foreach (var script in responseItem.ActionScripts) {
                            LuaEngine.ExecuteScript (script, responseItem);
                        }
                    }

                    if (responseItem.Character == null) {
                        return;
                    }

                    controller.PresentViewController (CreateControllerForConversation (items, localResponseId, speaker), false, null);

                    if (responseItem.ResponseIds == null) {
                        await Task.Delay (2500);

                        controller.DismissViewController (false, null);
                    }
                });
                alert.AddAction (a);
            }

            return alert;
        }

        async void RunConversation (NSNotification note) 
        {
            var userInfo = note.UserInfo;

            var v = (NSValue) userInfo[ConversationKey];
            var handle = (GCHandle) v.PointerValue;
            var conversation = handle.Target as ConversationItem[];

            var nsi = (NSNumber)userInfo [ConversationIdKey];
            var id = nsi.Int32Value;

            var charEntity = (Entity) userInfo [CharacterEntityKey];

            if (conversation [id].Player != null) {
                var text = string.Join ("\n", conversation [id].Player);
                var alert = UIAlertController.Create ("Player", text, UIAlertControllerStyle.Alert);
                controller.PresentViewController (alert, false, null);

                await Task.Delay (2500);

                controller.DismissViewController (false, null);
            }

            controller.PresentViewController (CreateControllerForConversation (conversation, id, charEntity), false, null);
        }

        public static void StartConversation (Entity playerEntity, Entity characterEntity, ConversationItem[] conversation, int id)
        {
            var handle = (IntPtr)GCHandle.Alloc (conversation);
            var conversationValue = NSValue.ValueFromPointer (handle);

            NSDictionary userInfo = new NSDictionary ((NSString)CharacterEntityKey, characterEntity,
                                                      (NSString)PlayerEntityKey, playerEntity,
                                                      ConversationIdKey, new NSNumber (id),
                                                      ConversationKey, conversationValue);
            var nc = NSNotificationCenter.DefaultCenter;

            nc.PostNotificationName (RunConversationNotification, characterEntity, userInfo);
        }
    }
}

