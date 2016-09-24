using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using Iceland.Characters;
namespace Iceland
{
    public class EntityClickHandler
    {
        UIViewController controller;

        public bool WaitingForEntityClick { get; set; }
        public Action<Entity> CustomClickHandler { get; set; }

        public EntityClickHandler (UIViewController viewController)
        {
            var nc = NSNotificationCenter.DefaultCenter;
            nc.AddObserver ((NSString)ClickableComponent.EntityClickedNotification, OnClicked);

            controller = viewController;
        }

        void OnClicked (NSNotification note)
        {
            var entity = (Entity)note.Object;

            if (CustomClickHandler != null) {
                CustomClickHandler (entity);
                return;
            }

            List<MenuDescription> mds = new List<MenuDescription> ();
            foreach (var c in entity.Components) {
                IMenuAction action = c as IMenuAction;
                if (action == null) {
                    continue;
                }

                if (action.OnlyIfCollected) {
                    continue;
                }

                MenuDescription md = new MenuDescription { Label = action.Label };
                md.Activated += (object sender, EventArgs e) => {
                    GameViewController.CurrentScene.Player.PerformAction (action, null, entity);
                };
                mds.Add (md);
            }

            if (mds.Count == 0) {
                return;
            }

            DisplayMenu (mds);
        }

        void DisplayMenu (List<MenuDescription> menuItems)
        {
            var alert = UIAlertController.Create ("", "", UIAlertControllerStyle.ActionSheet);

            foreach (var i in menuItems) {
                var localItem = i;
                var a = UIAlertAction.Create (i.Label, 0, (obj) => localItem.OnActivated ());
                alert.AddAction (a);
            }

            var cancelItem = UIAlertAction.Create ("Cancel", UIAlertActionStyle.Cancel, null);
            alert.AddAction (cancelItem);
            controller.PresentViewController (alert, true, null);
        }
    }
}

