using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;

using Iceland.Characters;
using Iceland.Map;

namespace Iceland
{
    public class GameScene : SKScene
    {
        Character player;
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
            mapNode.Position = new CoreGraphics.CGPoint (Frame.Width / 2, Frame.Height / 2);
            AddChild (mapNode);

            mapNode.AddCharacter (player);

            player.PositionCharacter (new Map.Map.Position { Row = 0, Column = 1 });

            player.Direction = Direction.South;
            player.Walking = true;
        }

        public override void TouchesBegan (NSSet touches, UIEvent evt)
        {
            UITouch touch = (UITouch)touches.AnyObject;
            CoreGraphics.CGPoint point = touch.LocationInNode (mapNode);

            Console.WriteLine ("{0} -> {1}", point, map.PointToPosition (point));
            player.PositionCharacter (map.PointToPosition (point));
        }

        public override void Update (double currentTime)
        {
            // Called before each frame is rendered
        }
    }
}

