using System;
using SpriteKit;
using GameplayKit;

namespace Iceland.Characters
{
    public class EntityNode : SKSpriteNode
    {
        public GKEntity Entity { get; set; }

        public override void TouchesEnded (Foundation.NSSet touches, UIKit.UIEvent evt)
        {
            var clickable = (ClickableComponent)Entity.GetComponent (typeof(ClickableComponent));
            if (clickable == null) {
                return;
            }

            clickable.DoClick ();
        }
    }
}

