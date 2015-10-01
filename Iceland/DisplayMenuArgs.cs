using System;

namespace Iceland
{
    public class DisplayMenuArgs : EventArgs
    {
        public MenuDescription[] menuItems { get; set; }
        public DisplayMenuArgs ()
        {
        }
    }
}

