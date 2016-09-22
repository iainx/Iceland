using System;
using System.Collections.Generic;

using UIKit;

using Iceland.Characters;
using Foundation;

namespace Iceland
{
    public class InventoryManager : UITableViewDataSource
    {
        InventoryComponent InvComp;

        public List<Entity> Items { get; private set; }

        public InventoryManager (InventoryComponent invComp)
        {
            Items = new List<Entity> ();

            InvComp = invComp;
            invComp.ItemAdded += (sender, e) => Items.Add (e.Item);
            invComp.ItemRemoved += (sender, e) => Items.Remove (e.Item);
        }

        public bool ContainsItemNamed (string id)
        {
            foreach (var i in Items) {
                if (i.Id == id) {
                    return true;
                }
            }

            return false;
        }

        public bool RemoveItemNamed (string id)
        {
            Console.WriteLine ($"Removing {id}");
            InvComp.RemoveItemNamed (id);

            return true;
        }

        public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell ("inventoryCell");
            if (cell == null) {
                cell = new UITableViewCell (UITableViewCellStyle.Default, "inventoryCell");
            }

            var item = Items [indexPath.Row];
            cell.TextLabel.Text = item.Name;

            return cell;
        }

        public override nint RowsInSection (UITableView tableView, nint section)
        {
            return Items.Count;
        }
    }
}

