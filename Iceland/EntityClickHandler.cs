using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;

using Iceland.Characters;
namespace Iceland
{
    public class EntityClickHandler
    {
        public event EventHandler<DisplayMenuArgs> DisplayMenu;

        public EntityClickHandler ()
        {
            var nc = NSNotificationCenter.DefaultCenter;
            nc.AddObserver ((NSString)ClickableComponent.EntityClickedNotification, OnClicked);
        }

        void OnClicked (NSNotification note)
        {
            var entity = (CharacterEntity)note.Object;

            if (DisplayMenu == null) {
                return;
            }

            List<MenuDescription> mds = new List<MenuDescription> ();
            foreach (var c in entity.Components) {
                IMenuAction action = c as IMenuAction;
                if (action == null) {
                    continue;
                }

                MenuDescription md = new MenuDescription { Label = action.Label };
                md.Activated += (object sender, EventArgs e) => {
                    MoveComponent comp = (MoveComponent)GameScene.player.GetComponent (typeof (MoveComponent));
                    comp.MoveEntity (new Map.Map.Position {Row=7, Column = 3}, () => action.Activate ());
                };
                mds.Add (md);
            }

            if (mds.Count == 0) {
                return;
            }

            DisplayMenu (this, new DisplayMenuArgs { menuItems = mds.ToArray () });
        }
    }
}

