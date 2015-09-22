using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;

using Iceland.Characters;
using Iceland.Map;

using GameplayKit;

namespace Iceland
{
    public class GameScene : SKScene
    {
        CharacterEntity player;
        Map.Map map;
        MapNode mapNode;

        public GameScene (IntPtr handle) : base (handle)
        {
            player = new Professor ();
            map = Map.Map.LoadFromFile ("pond.tmx");
        }

        public override void DidMoveToView (SKView view)
        {
            mapNode = new MapNode (map);
            mapNode.Position = new CoreGraphics.CGPoint (Frame.Width / 2 - 200, Frame.Height / 2 + 200);
            AddChild (mapNode);

            player.CurrentPosition = new Map.Map.Position { Row = 0, Column = 1 };
            player.Map = map;
            mapNode.AddCharacter (player);

            var comp = (CharacterSpriteComponent)player.GetComponent (typeof(CharacterSpriteComponent));
            comp.Direction = Direction.North;
            comp.Walking = false;
        }

        public override void TouchesBegan (NSSet touches, UIEvent evt)
        {
            UITouch touch = (UITouch)touches.AnyObject;
            CoreGraphics.CGPoint point = touch.LocationInNode (mapNode);

            Map.Map.Position destination = map.PointToPosition (point);
            if (!map.PositionIsValid (destination)) {
                return;
            }

            GKGraphNode[] path = map.FindPath (player.CurrentPosition, destination);

            CharacterSpriteComponent comp = (CharacterSpriteComponent)player.GetComponent (typeof(CharacterSpriteComponent));
            comp.FollowPath (path, () => Console.WriteLine ("Completed"));
        }

        public override void Update (double currentTime)
        {
            // Called before each frame is rendered
        }
    }
}

