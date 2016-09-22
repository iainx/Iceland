// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Iceland
{
    [Register ("InventoryViewController")]
    partial class InventoryViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView InventoryTable { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (InventoryTable != null) {
                InventoryTable.Dispose ();
                InventoryTable = null;
            }
        }
    }
}