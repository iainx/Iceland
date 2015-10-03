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

            Size = new CoreGraphics.CGSize (map.Width * 100, map.Height * 50 + 300);
            AnchorPoint = new CGPoint (0.5, 1);
        }

        void SetCameraConstraints (SKCameraNode camera, SKSpriteNode sprite)
        {
            // Constrain the camera to the player
            var zeroRange = new SKRange (0, 0);
            SKConstraint playerConstraint = SKConstraint.CreateDistance (zeroRange, sprite);

            var scaledSize = new CGSize (Size.Width * camera.XScale, Size.Height * camera.YScale);
            var contentRect = mapNode.CalculateAccumulatedFrame ();
            contentRect = new CGRect (contentRect.X, contentRect.Y, contentRect.Width, 900);
                
            nfloat xInset = (nfloat)Math.Min ((scaledSize.Width / 2.0) + 10, contentRect.Width / 2.0);
            nfloat yInset = (nfloat)Math.Min ((scaledSize.Height / 2.0) + 10, contentRect.Height / 2.0);

            var insetRect = contentRect.Inset (xInset, yInset);

            var xRange = new SKRange (insetRect.GetMinX (), insetRect.GetMaxX ());
            var yRange = new SKRange (insetRect.GetMinY (), insetRect.GetMaxY ());

            var levelEdgeConstraint = SKConstraint.CreateRestriction (xRange, yRange);
            levelEdgeConstraint.ReferenceNode = this;

            camera.Constraints = new[] { playerConstraint, levelEdgeConstraint };
        }

        public override void DidMoveToView (SKView view)
        {
            cameraNode = new SKCameraNode ();
            Camera = cameraNode;
            Camera.XScale = 1.0f;
            Camera.YScale = 1.0f;
            AddChild (cameraNode);

            mapNode = new MapNode (map);
            mapNode.Position = new CGPoint (mapNode.Position.X, mapNode.Position.Y + 100);
            AddChild (mapNode);

            mapNode.MapClicked += new EventHandler<MapClickedArgs>(HandleTouchOnMap);

            player.CurrentPosition = new Map.Map.Position { Row = 0, Column = 0 };
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

