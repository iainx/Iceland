using System;
using SpriteKit;

using Iceland.Map;

namespace Iceland.Characters
{
    public class Character : SKSpriteNode
    {
        SKAction idleNorthEastAction;
        SKAction idleSouthWestAction;
        SKAction walkNorthEastAction;
        SKAction walkSouthWestAction;

        Direction direction;
        public Direction Direction {
            get {
                return direction;
            }

            set {
                direction = value;
                UpdateCharacterAnimation ();
            }
        }

        bool walking;
        public bool Walking {
            get {
                return walking;
            }

            set {
                walking = value;
                UpdateCharacterAnimation ();
            }
        }

        public Map.Map OwningMap { get; set; }
        Map.Map.Position currentPosition;
        public Map.Map.Position CurrentPosition { 
            get { 
                return currentPosition;
            } 

            set {
                CoreGraphics.CGPoint point = OwningMap.PositionToPoint (value, true);
                Position = point;
                ZPosition = OwningMap.ZLevelForPosition (value);
                currentPosition = value;
            }
        }

        SKTexture[] TextureArrayFromAtlas (SKTextureAtlas atlas, string name, int row)
        {
            SKTexture[] textures = new SKTexture[8];
            for (int i = 0; i < 8; i++) {
                Console.WriteLine (String.Format ("{0}-{1}-{2}", name, row, i + 1));
                textures[i] = atlas.TextureNamed (String.Format ("{0}-{1}-{2}", name, row, i + 1));
            }
            return textures;
        }

        public Character (string spriteAtlasFilename)
        {
            // Setup the idle actions
            SKTextureAtlas atlas = SKTextureAtlas.FromName (spriteAtlasFilename);
            idleNorthEastAction = SKAction.AnimateWithTextures (new SKTexture[] { atlas.TextureNamed (spriteAtlasFilename + "-1-0") }, 1.0);
            idleSouthWestAction = SKAction.AnimateWithTextures (new SKTexture[] { atlas.TextureNamed (spriteAtlasFilename + "-3-0") }, 1.0);

            // Setup the walking action
            SKTexture[] textArray = TextureArrayFromAtlas (atlas, spriteAtlasFilename, 1);
            walkNorthEastAction = SKAction.AnimateWithTextures (textArray, 0.1);
            textArray = TextureArrayFromAtlas (atlas, spriteAtlasFilename, 3);
            walkSouthWestAction = SKAction.AnimateWithTextures (textArray, 0.1);

            // Set the defaults
            Size = new CoreGraphics.CGSize (64f, 64f);
            Texture = atlas.TextureNamed (spriteAtlasFilename + "-1-0");
            AnchorPoint = new CoreGraphics.CGPoint (0.5f, 0.0f);
        }

        void UpdateCharacterAnimation ()
        {
            int realDirection = (Direction == Direction.North || Direction == Direction.East) ? 3 : 1;
            if (walking) {
                RemoveActionForKey ("standingAction");

                SKAction walkAction = (realDirection == 1) ? walkSouthWestAction : walkNorthEastAction;
                RunAction (SKAction.RepeatActionForever (walkAction), "standingAction");
            } else {
                RemoveActionForKey ("standingAction");
                RunAction (idleNorthEastAction, "standingAction");
            }
        }

        public void MoveCharacter (Map.Map.Position position, Action completionHandler)
        {
            
        }
    }
}

