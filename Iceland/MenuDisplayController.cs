using System;
using System.Collections.Generic;

using UIKit;

using Iceland.Characters;

namespace Iceland
{
    public static class MenuDisplayController
    {
        public static UIViewController DisplayMenuForItem (Entity item, UIViewController parentController, EntityClickHandler clickHandler)
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
                    if (action.NumberOfEntitiesNeeded == 1) {
                        GameViewController.CurrentScene.Player.PerformAction (action, null, item);
                    } else {
                        clickHandler.WaitingForEntityClick = true;
                        clickHandler.CustomClickHandler = (obj) => {
                            GameViewController.CurrentScene.Player.PerformAction (action, item, obj);
                            clickHandler.CustomClickHandler = null;
                            clickHandler.WaitingForEntityClick = false;
                        };
                    }
                    /*
                    MoveComponent comp = (MoveComponent)GameViewController.CurrentScene.Player.GetComponent (typeof (MoveComponent));
                    comp.MoveEntity (item.Model.ActionPosition, (bool success) => {
                        if (success == false) {
                            Console.WriteLine ("Can't do that");
                            return;
                        }
                        if (action.NumberOfEntitiesNeeded == 1) {
                            action.Activate (GameViewController.CurrentScene.Player, null);

                        } else {
                            clickHandler.WaitingForEntityClick = true;
                            clickHandler.CustomClickHandler = (obj) => {
                                action.Activate (GameViewController.CurrentScene.Player, obj);
                                clickHandler.CustomClickHandler = null;
                                clickHandler.WaitingForEntityClick = false;
                            };
                        }
                    });
                    */
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

