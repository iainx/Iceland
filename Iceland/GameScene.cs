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
            mapNode.Position = new CGPoint (Frame.Width / 2 - 200, Frame.Height / 2 + 200);
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

