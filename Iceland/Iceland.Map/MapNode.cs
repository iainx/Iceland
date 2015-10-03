using System;
using System.Collections.Generic;

using Foundation;
using UIKit;
using SpriteKit;
using Iceland.Characters;

namespace Iceland.Map
{
    public class MapNode : SKNode
    {
        Map map;
        SKTextureAtlas atlas;

        public MapNode (Map map)
        {
            this.map = map;
            Name = "Map";

            UserInteractionEnabled = true;

            int idx = 0;

            atlas = SKTextureAtlas.FromName ("TileImages");
            foreach (var tid in map.TIDs) {
                Tile t = map.TIDToTile [tid];
                CoreGraphics.CGPoint position = map.PositionToPoint (map.IndexToPosition (idx));

                SKTexture tex = atlas.TextureNamed (t.ImageName);
                SKSpriteNode node = SKSpriteNode.FromTexture (tex);
                node.UserInteractionEnabled = false;
                node.Position = position;
                node.ZPosition = idx;

                node.AnchorPoint = new CoreGraphics.CGPoint (0.5, 0);
                AddChild (node);

                idx++;
            }
        }

        public void AddCharacter (CharacterEntity character)
        {
            CharacterSpriteComponent spriteComp = (CharacterSpriteComponent)character.GetComponent (typeof (CharacterSpriteComponent));
            AddChild (spriteComp.Sprite);

            CoreGraphics.CGPoint point = map.PositionToPoint (character.CurrentPosition, true);
            spriteComp.Sprite.Position = point;
            spriteComp.Sprite.ZPosition = map.ZLevelForPosition (character.CurrentPosition);
        }
            
        public event EventHandler<MapClickedArgs> MapClicked;

        public override void TouchesEnded (NSSet touches, UIEvent evt)
        {
            base.TouchesEnded (touches, evt);

            if (MapClicked != null) {
                UITouch touch = (UITouch)touches.AnyObject;
                CoreGraphics.CGPoint point = touch.LocationInNode (this);
                Map.Position destination = map.PointToPosition (point);
                if (!map.PositionIsValid (destination)) {
                    return;
                }
                MapClickedArgs args = new MapClickedArgs (destination);
                MapClicked (this, args);
            }
        }
    }

    public class MapClickedArgs:EventArgs
    {
        public Map.Position ClickPosition { get; private set; }
        public MapClickedArgs (Map.Position pos) {
            ClickPosition = pos;
        }
    }
}

