using System;

using Foundation;
using UIKit;

using Iceland.Characters;
using System.Threading.Tasks;

namespace Iceland
{
    public class LookHandler
    {
        public static readonly string LookNotification = "LookHandler.RunLookNotification";
        public static readonly string PlayerEntityKey = "LookHandler.PlayerEntityKey";
        public static readonly string ItemEntityKey = "LookHandler.ItemEntityKey";

        UIViewController controller;

        public LookHandler (UIViewController mainController)
        {
            var nc = NSNotificationCenter.DefaultCenter;
            nc.AddObserver ((NSString)LookNotification, HandleLookNotification);

            controller = mainController;
        }

        async void HandleLookNotification (NSNotification note)
        {
            var userInfo = note.UserInfo;

            var itemEntity = (Entity)userInfo [ItemEntityKey];

            var description = (string)LuaEngine.ExecuteScript (itemEntity.Model.LookCommand)[0];

            var alert = UIAlertController.Create ("", description, UIAlertControllerStyle.Alert);
            controller.PresentViewController (alert, false, null);

            await Task.Delay (5000);

            controller.DismissViewController (false, null);
        }

        public static void StartLook (Entity playerEntity, Entity itemEntity)
        {
            var userInfo = new NSDictionary ((NSString)PlayerEntityKey, playerEntity,
                                             (NSString)ItemEntityKey, itemEntity);
            var nc = NSNotificationCenter.DefaultCenter;
            nc.PostNotificationName (LookNotification, itemEntity, userInfo);
        }
    }
}

