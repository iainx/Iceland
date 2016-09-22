using System;

using Foundation;
using UIKit;

using Iceland.Characters;

namespace Iceland
{
    public partial class InventoryViewController : UIViewController, IUITableViewDelegate
    {
        public event EventHandler<InventoryItemActivatedArgs> ItemActivated;

        public InventoryManager Manager { get; set; }

        public InventoryViewController () : base ("InventoryViewController", null)
        {
            ModalPresentationStyle = UIModalPresentationStyle.Popover;
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            // Perform any additional setup after loading the view, typically from a nib.

            InventoryTable.DataSource = Manager;
            InventoryTable.Delegate = this;
        }

        public override void DidReceiveMemoryWarning ()
        {
            base.DidReceiveMemoryWarning ();
            // Release any cached data, images, etc that aren't in use.
        }

        [Export ("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected (UITableView tableView, NSIndexPath indexPath)
        {
            Entity item = Manager.Items [indexPath.Row];
            ItemActivated?.Invoke (this, new InventoryItemActivatedArgs (item));
        }
    }

    public class InventoryItemActivatedArgs : EventArgs
    {
        public Entity Item { get; private set; }

        public InventoryItemActivatedArgs (Entity item)
        {
            Item = item;
        }
    }
}


