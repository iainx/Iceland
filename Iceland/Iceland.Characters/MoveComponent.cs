using System;
using System.Collections.Generic;
using System.Linq;

using SpriteKit;
using GameplayKit;

using Iceland.Extensions;
using Iceland.Map;

namespace Iceland.Characters
{
    public class MoveComponent : GKComponent
    {
        Queue<SKAction> currentWalk;
        bool teleportation = false;

        public void MoveEntity (Map.Map.Position destination, Action<bool> completionHandler)
        {
            var centity = (Entity)Entity;
            var comp = (CharacterSpriteComponent)Entity.GetComponent (typeof(CharacterSpriteComponent));

            if (comp == null) {
                return;
            }

            GKGraphNode[] path = GameViewController.CurrentScene.CurrentMap.FindPath (centity.Model.StartPosition, destination, false);

            if (teleportation) {
                if (path == null) {
                    completionHandler (false);
                    return;
                }

                var lastNode = (MapGraphNode) path.Last ();
                CoreGraphics.CGPoint point = GameViewController.CurrentScene.CurrentMap.PositionToPoint (lastNode.NodePosition, true);
                comp.Sprite.Position = point;
                centity.Model.StartPosition = lastNode.NodePosition;
                comp.Sprite.ZPosition = GameViewController.CurrentScene.CurrentMap.ZLevelForPosition (lastNode.NodePosition);

                completionHandler (true);
            } else {
                FollowPath (path, completionHandler);
            }
        }

        #region Movement

        static readonly string WalkingKey = "characterWalking";

        public void FollowPath (GKGraphNode[] path, Action<bool> completion)
        {
            if (path == null) {
                completion (false);
                return;
            }

            CharacterSpriteComponent comp = Entity.GetComponent<CharacterSpriteComponent> ();
            if (comp == null) {
                completion (false);
                return;
            }

            var dropFirst = path.Skip (1);
            Queue<SKAction> sequence = new Queue<SKAction> ();
            var sprite = comp.Sprite;

            sprite.RemoveActionForKey (WalkingKey);

            foreach (MapGraphNode node in dropFirst) {
                Entity centity = (Entity)Entity;
                CoreGraphics.CGPoint point = GameViewController.CurrentScene.CurrentMap.PositionToPoint (node.NodePosition, true);
                List<SKAction> subseq = new List<SKAction> ();

                subseq.Add (SKAction.Run (() => {
                    comp.Direction = centity.Model.StartPosition.DirectionToPosition (node.NodePosition);
                    centity.Model.StartPosition = node.NodePosition;
                    sprite.ZPosition = GameViewController.CurrentScene.CurrentMap.ZLevelForPosition (node.NodePosition);
                    comp.Walking = true;
                }));

                subseq.Add (SKAction.MoveTo (point, 0.8));

                sequence.Enqueue (SKAction.Sequence (subseq.ToArray ()));
            }

            sequence.Enqueue (SKAction.Run (() => comp.Walking = false));
            if (completion != null) {
                sequence.Enqueue (SKAction.Run (() => completion (true)));
            }

            currentWalk = sequence;
            if (!comp.Walking) {
                RunNextStep (sprite);
            }
        }
        #endregion

        void RunNextStep (SKSpriteNode sprite)
        {
            if (currentWalk.Count == 0) {
                return;
            }

            List<SKAction> actions = new List<SKAction> ();

            // Pull the next step off the queue
            actions.Add (currentWalk.Dequeue ());
            actions.Add (SKAction.Run (() => {
                RunNextStep (sprite);
            }));

            sprite.RunAction (SKAction.Sequence (actions.ToArray ()));
        }
    }
}