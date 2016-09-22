using System;
using GameplayKit;
using SpriteKit;
using UIKit;

namespace Iceland.Characters
{
    public class ItemSpriteComponent : GKComponent
    {
        public SKSpriteNode Sprite { get; private set; }

        public ItemSpriteComponent (SKTexture texture, GKEntity entity)
        {
            Sprite = new EntityNode ();
            Sprite.Texture = texture;
            Sprite.Size = new CoreGraphics.CGSize (64f, 64f);
            Sprite.AnchorPoint = new CoreGraphics.CGPoint (0.5f, 0.0f);
            Sprite.UserInteractionEnabled = true;
            ((EntityNode)Sprite).Entity = entity;
        }
    }
}

