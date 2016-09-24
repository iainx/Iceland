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

        public int NumberOfEntitiesNeeded {
            get {
                return 1; // FIXME: This actually depends on the item. Some items may need to be used with another object.
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

