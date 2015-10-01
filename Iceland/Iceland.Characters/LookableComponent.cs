using System;

using GameplayKit;

namespace Iceland.Characters
{
    public class LookableComponent : GKComponent, IMenuAction
    {
        public string Label { 
            get { return "Look at"; } 
        }

        public LookableComponent ()
        {
        }

        public void Activate ()
        {
            Console.WriteLine ("Looking at");
        }
    }
}

