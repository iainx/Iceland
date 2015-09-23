﻿using System;
using System.Collections.Generic;
using System.Linq;
using SpriteKit;
using GameplayKit;

using Iceland.Map;

namespace Iceland.Characters
{
    public class CharacterSpriteComponent : GKComponent
    {
        public SKSpriteNode Sprite { get; private set; }
        SKAction idleNorthWestAction;
        SKAction idleSouthEastAction;
        SKAction walkNorthWestAction;
        SKAction walkSouthEastAction;

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

        public CharacterSpriteComponent (string spriteAtlasFilename)
        {
            // Setup the idle actions
            SKTextureAtlas atlas = SKTextureAtlas.FromName (spriteAtlasFilename);
            idleNorthWestAction = SKAction.AnimateWithTextures (new SKTexture[] { atlas.TextureNamed (spriteAtlasFilename + "-1-0") }, 1.0);
            idleSouthEastAction = SKAction.AnimateWithTextures (new SKTexture[] { atlas.TextureNamed (spriteAtlasFilename + "-3-0") }, 1.0);

            // Setup the walking action
            SKTexture[] textArray = TextureArrayFromAtlas (atlas, spriteAtlasFilename, 1);
            walkNorthWestAction = SKAction.AnimateWithTextures (textArray, 0.1);
            textArray = TextureArrayFromAtlas (atlas, spriteAtlasFilename, 3);
            walkSouthEastAction = SKAction.AnimateWithTextures (textArray, 0.1);

            Sprite = new SKSpriteNode ();
            Sprite.Texture = atlas.TextureNamed (spriteAtlasFilename + "-1-0");
            Sprite.Size = new CoreGraphics.CGSize (64f, 64f);
            Sprite.AnchorPoint = new CoreGraphics.CGPoint (0.5f, 0.0f);
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

        void UpdateCharacterAnimation ()
        {
            int realDirection = (Direction == Direction.North || Direction == Direction.West) ? 3 : 1;
            if (walking) {
                Sprite.RemoveActionForKey ("standingAction");

                SKAction walkAction = (realDirection == 1) ? walkSouthEastAction : walkNorthWestAction;
                Sprite.RunAction (SKAction.RepeatActionForever (walkAction), "standingAction");
            } else {
                Sprite.RemoveActionForKey ("standingAction");
                SKAction idleAction = (realDirection == 1) ? idleSouthEastAction : idleNorthWestAction;
                Sprite.RunAction (idleAction, "standingAction");
            }
        }

        #region Movement

        public void FollowPath (GKGraphNode[] path, Action completion)
        {
            var dropFirst = path.Skip (1);
            List<SKAction> sequence = new List<SKAction> ();

            foreach (MapGraphNode node in dropFirst) {
                CharacterEntity centity = (CharacterEntity)Entity;
                CoreGraphics.CGPoint point = centity.Map.PositionToPoint (node.NodePosition, true);
            
                sequence.Add (SKAction.Run (() => {
                    Direction = centity.CurrentPosition.DirectionToPosition (node.NodePosition);
                    Console.WriteLine ("{0} -> {1}: {2}", centity.CurrentPosition, node.NodePosition, Direction);
                    Walking = true;
                }));

                sequence.Add (SKAction.MoveTo (point, 0.8));
                sequence.Add (SKAction.Run (() => {
                    centity.CurrentPosition = node.NodePosition;
                    Sprite.ZPosition = centity.Map.ZLevelForPosition (node.NodePosition);
                    Walking = false;
                }));
            }

            sequence.Add (SKAction.Run (completion));
            Sprite.RunAction (SKAction.Sequence (sequence.ToArray ()));
        }
        #endregion
    }
}
