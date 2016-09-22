using System;

using GameplayKit;

namespace Iceland.Characters
{
    public class UseableComponent : GKComponent, IMenuAction
    {
        public string Label {
            get {
                return "Use";
            }
        }

        public bool OnlyIfCollected {
            get {
                return true;
            }
        }

        public bool OnlyIfDropped {
            get {
                return false;
            }
        }

        public void Activate (Entity playerEntity, Entity otherEntity)
        {
            throw new NotImplementedException ();
        }
    }
}

