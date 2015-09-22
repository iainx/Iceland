using System;
using System.Collections.Generic;

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

            int idx = 0;

            atlas = SKTextureAtlas.FromName ("TileImages");
            foreach (var tid in map.TIDs) {
                Tile t = map.TIDToTile [tid];
                CoreGraphics.CGPoint position = map.PositionToPoint (map.IndexToPosition (idx));

                SKTexture tex = atlas.TextureNamed (t.ImageName);
                SKSpriteNode node = SKSpriteNode.FromTexture (tex);
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
    }
}

