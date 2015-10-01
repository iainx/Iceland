using System;
using GameplayKit;

namespace Iceland.Characters
{
    public class TalkableComponent : GKComponent, IMenuAction
    {
        public string Label { 
            get { return "Talk to"; } 
        }

        public TalkableComponent ()
        {
        }

        public void Activate ()
        {
            Console.WriteLine ("Talking to");
        }
    }
}

