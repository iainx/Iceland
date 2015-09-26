using System;
using GameplayKit;
using Foundation;

namespace Iceland.Characters
{
    public class ClickableComponent : GKComponent
    {
        public ClickableComponent ()
        {
        }

        public void DoClick ()
        {
            var nc = NSNotificationCenter.DefaultCenter;
            nc.PostNotificationName ("EntityClicked", Entity);
        }
    }
}

