using System;
using System.Collections.Generic;

using UIKit;

using Iceland.Characters;

namespace Iceland
{
    public static class MenuDisplayController
    {
        public static UIViewController DisplayMenuForItem (Entity item, UIViewController parentController)
        {
            if (item.Components.Length == 0) {
                return null;
            }

            var alert = UIAlertController.Create ("", "", UIAlertControllerStyle.ActionSheet);

            foreach (var c in item.Components) {
                IMenuAction action = c as IMenuAction;
                if (action == null) {
                    continue;
                }

                if (action.OnlyIfDropped) {
                    continue;
                }

                MenuDescription md = new MenuDescription { Label = action.Label };

                md.Activated += (object sender, EventArgs e) => {
                    MoveComponent comp = (MoveComponent)GameViewController.CurrentScene.Player.GetComponent (typeof (MoveComponent));
                    comp.MoveEntity (item.Model.ActionPosition, (bool success) => {
                        if (success == false) {
                            Console.WriteLine ("Can't do that");
                            return;
                        }
                        action.Activate (GameViewController.CurrentScene.Player, null);
                    });
                };

                var a = UIAlertAction.Create (md.Label, 0, (obj) => md.OnActivated ());
                alert.AddAction (a);
            }

            var cancelItem = UIAlertAction.Create ("Cancel", UIAlertActionStyle.Cancel, null);
            alert.AddAction (cancelItem);

            parentController.PresentViewController (alert, false, null);

            return alert;
        }
    }
}

