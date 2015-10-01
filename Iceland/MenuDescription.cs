using System;

namespace Iceland
{
    public class MenuDescription
    {
        public string Label { get; set; }
        public event EventHandler Activated;

        public MenuDescription ()
        {
        }

        public void OnActivated ()
        {
            if (Activated != null) {
                Activated (this, EventArgs.Empty);
            }
        }
    }
}

