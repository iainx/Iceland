using System;

using Foundation;
using UIKit;

using Iceland.Characters;

namespace Iceland
{
    public class GiveHandler
    {
        public static readonly string GiveNotification = "GiveHandler.RunGiveNotification";
        public static readonly string PlayerEntityKey = "GiveHandler.PlayerEntityKey";
        public static readonly string RecipientEntityKey = "GiveHandler.RecipientEntityKey";
        public static readonly string ItemEntityKey = "GiveHandler.ItemEntityKey";

        UIViewController controller;

        public GiveHandler (UIViewController mainController)
        {
            var nc = NSNotificationCenter.DefaultCenter;
            nc.AddObserver ((NSString)GiveNotification, HandleGiveNotification);

            controller = mainController;
        }

        void HandleGiveNotification (NSNotification note)
        {
            
        }

        public static void StartGive (Entity playerEntity, Entity itemEntity, Entity recepientEntity)
        {
            var userInfo = new NSDictionary ((NSString)PlayerEntityKey, playerEntity,
                                             (NSString)RecipientEntityKey, recepientEntity,
                                             (NSString)ItemEntityKey, itemEntity);
            var nc = NSNotificationCenter.DefaultCenter;

            Console.WriteLine ($"Giving {itemEntity.Name} to {recepientEntity.Name}");
            nc.PostNotificationName (GiveNotification, itemEntity, userInfo);    
        }
    }
}

