using System;
using System.Collections.Generic;

using SpriteKit;
using Iceland.Characters;

namespace Iceland.Map
{
    public class MapNode : SKNode
    {
        Map map;

        public MapNode (Map map)
        {
            this.map = map;

            int idx = 0;

            foreach (var tid in map.TIDs) {
                Tile t = map.TIDToTile [tid];
                CoreGraphics.CGPoint position = map.PositionToPoint (map.IndexToPosition (idx));

                SKSpriteNode node = SKSpriteNode.FromImageNamed (t.ImageName);
                node.Position = position;
                node.ZPosition = idx;
                node.AnchorPoint = new CoreGraphics.CGPoint (0.5, 0);
                AddChild (node);

                idx++;
            }
        }

        public void AddCharacter (Character character)
        {
            AddChild (character);
            character.OwningMap = map;
        }
    }
}

