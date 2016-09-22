using System;

using System.Threading.Tasks;

using Foundation;
using UIKit;

using Iceland.Characters;
using Iceland.Extensions;

namespace Iceland
{
    public class CollectHandler
    {
        public static readonly string CollectNotification = "CollectHandler.RunCollectNotification";
        public static readonly string PlayerEntityKey = "CollectHandler.PlayerEntityKey";
        public static readonly string ItemEntityKey = "CollectHandler.ItemEntityKey";

        UIViewController controller;

        public CollectHandler (UIViewController mainController)
        {
            var nc = NSNotificationCenter.DefaultCenter;
            nc.AddObserver ((NSString)CollectNotification, HandleCollectNotification);

            controller = mainController;
        }

        async void HandleCollectNotification (NSNotification note)
        {
            var userInfo = note.UserInfo;

            var itemEntity = (Entity)userInfo [ItemEntityKey];
            var playerEntity = (Entity)userInfo [PlayerEntityKey];

            var invComponent = playerEntity.GetComponent<InventoryComponent> ();
            if (invComponent == null) {
                throw new InvalidOperationException ("No inventory component");
            }

            bool add = true;
            if (!string.IsNullOrEmpty (itemEntity.Model.CollectCommand)) {
                Console.WriteLine ($"{itemEntity.Model.CollectCommand}");
                var results = LuaEngine.ExecuteScript (itemEntity.Model.CollectCommand);
                Console.WriteLine ("Parsed");
                add = (bool)results [1];

                Console.WriteLine ($"{add}");
                if (!string.IsNullOrEmpty ((string)results [0])) {
                    var alert = UIAlertController.Create ("", (string)results [0], UIAlertControllerStyle.Alert);
                    controller.PresentViewController (alert, false, null);

                    await Task.Delay (5000);

                    controller.DismissViewController (false, null);
                }
            }

            if (add) {
                invComponent.AddItem (itemEntity);
            }
        }

        public static void StartCollect (Entity playerEntity, Entity itemEntity)
        {
            var userInfo = new NSDictionary ((NSString)PlayerEntityKey, playerEntity,
                                             (NSString)ItemEntityKey, itemEntity);
            var nc = NSNotificationCenter.DefaultCenter;
            nc.PostNotificationName (CollectNotification, itemEntity, userInfo);
        }
    }
}

