using System;

namespace Iceland
{
    public class DisplayMenuArgs : EventArgs
    {
        public string Title { get; set; }
        public MenuDescription[] menuItems { get; set; }
        public DisplayMenuArgs ()
        {
        }
    }
}

