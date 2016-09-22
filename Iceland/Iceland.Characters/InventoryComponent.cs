using System;
using System.Collections.Generic;
using GameplayKit;

using Foundation;

namespace Iceland.Characters
{
    public class InventoryComponent : GKComponent
    {
        public static readonly string ItemCollectedNotification = "ItemCollected";
        public static readonly string ItemDroppedNotification = "ItemDropped";

        public event EventHandler<InventoryEventArgs> ItemAdded;
        public event EventHandler<InventoryEventArgs> ItemRemoved;
        Dictionary<string, Entity> items;

        public InventoryComponent ()
        {
            items = new Dictionary<string, Entity> ();
        }

        public void AddItem (Entity item)
        {
            items [item.Id] = item;
            ItemAdded?.Invoke (this, new InventoryEventArgs (item));

            var nc = NSNotificationCenter.DefaultCenter;
            nc.PostNotificationName (ItemCollectedNotification, item);
        }

        public void RemoveItemNamed (string id)
        {
            var item = items [id];
            items.Remove (id);
            ItemRemoved?.Invoke (this, new InventoryEventArgs (item));
        }

        public bool HasItemNamed (string id)
        {
            return items.ContainsKey (id);
        }
    }

    public class InventoryEventArgs : EventArgs
    {
        public Entity Item { get; private set; }

        public InventoryEventArgs (Entity item)
        {
            Item = item;
        }
    }
}

