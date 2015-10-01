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
        public static CharacterEntity player;
        CharacterEntity skeleton;
        Map.Map map;
        MapNode mapNode;
        SKCameraNode cameraNode;

        public GameScene (IntPtr handle) : base (handle)
        {
            player = new Professor ();
            skeleton = new Skeleton ();
            map = Map.Map.LoadFromFile ("pond.tmx");
        }

        void SetCameraConstraints (SKCameraNode camera, SKSpriteNode sprite)
        {
            var zeroRange = new SKRange (0, 200);
            SKConstraint playerConstraint = SKConstraint.CreateDistance (zeroRange, sprite);
            camera.Constraints = new SKConstraint[]{ playerConstraint };
        }

        public override void DidMoveToView (SKView view)
        {
            cameraNode = new SKCameraNode ();
            Camera = cameraNode;
            AddChild (cameraNode);

            mapNode = new MapNode (map);
            mapNode.Position = new CGPoint ((Frame.Width - mapNode.Frame.Width) / 2, Frame.Height / 2 + 200);
            AddChild (mapNode);

            mapNode.MapClicked += new EventHandler<MapClickedArgs>(HandleTouchOnMap);

            player.CurrentPosition = new Map.Map.Position { Row = 0, Column = 1 };
            player.Map = map;
            mapNode.AddCharacter (player);

            var comp = (CharacterSpriteComponent)player.GetComponent (typeof(CharacterSpriteComponent));
            comp.Direction = Direction.North;
            comp.Walking = false;

            skeleton.CurrentPosition = new Map.Map.Position { Row = 6, Column = 3 };
            skeleton.Map = map;
            mapNode.AddCharacter (skeleton);

            SetCameraConstraints (cameraNode, comp.Sprite);
        }

        void HandleTouchOnMap (object sender, MapClickedArgs args)
        {
            Map.Map.Position destination = args.ClickPosition;
            MoveComponent comp = (MoveComponent)player.GetComponent (typeof(MoveComponent));
            comp.MoveEntity (destination, null);
        }

        public override void Update (double currentTime)
        {
            // Called before each frame is rendered
        }
    }
}

