using System;
using System.Threading;
using Foundation;
using Iceland.Characters;

using UIKit;

namespace Iceland
{
    public class ConversationHandler
    {
        UIViewController controller;
        Timer conversationTimer;

        public ConversationHandler (UIViewController mainController)
        {
            var nc = NSNotificationCenter.DefaultCenter;
            nc.AddObserver ((NSString)TalkableComponent.RunConversationNotification, RunConversation);

            controller = mainController;
        }

        UIViewController CreateControllerForConversation (Conversation.Details details, Conversation.Item item)
        {
            var alert = UIAlertController.Create ("Player", item.Text, UIAlertControllerStyle.Alert);
            foreach (var response in item.Responses) {
                var localResponse = response;
                var a = UIAlertAction.Create (response.Text, 0, (obj) => { 
                    controller.DismissViewController (false, null);

                    if (localResponse.NextItem == -1) {
                        return;
                    }

                    controller.PresentViewController (CreateControllerForConversation (details, details.Items[localResponse.NextItem]), false, null);
                });
                alert.AddAction (a);
            }

            return alert;
        }

        void RunConversation (NSNotification note) 
        {
            var userInfo = note.UserInfo;

            var conversation = (Conversation.Details)userInfo[TalkableComponent.ConversationKey];
            var alert = UIAlertController.Create ("Player", conversation.Introduction, UIAlertControllerStyle.Alert);
            controller.PresentViewController (alert, false, null);

            conversationTimer = new Timer ((object state) => controller.InvokeOnMainThread (() => {
                controller.DismissViewController (false, null);
                controller.PresentViewController (CreateControllerForConversation (conversation, conversation.Items [0]), false, null);
            }), this, 5000, Timeout.Infinite);
        }
    }
}

