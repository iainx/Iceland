using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;

using Iceland.Characters;
using Iceland.Extensions;
using Iceland.Map;

using GameplayKit;

namespace Iceland
{
    public class GameScene : SKScene
    {
        public Entity Player { get; private set; }
        public Map.Map CurrentMap { get; private set; }
        public InventoryManager InvManager { get; private set; }

        MapNode mapNode;
        SKCameraNode cameraNode;

        public GameScene (IntPtr handle) : base (handle)
        {
            Player = new Professor ();
            var invComp = Player.GetComponent<InventoryComponent> ();
            InvManager = new InventoryManager (invComp);

            LuaEngine.SetGlobalVariable ("inventory", InvManager);
            LuaEngine.SetGlobalVariable ("game", this);

            try {
                LuaEngine.ExecuteScriptFromFile ("Main.lua");
            } catch (Exception e) {
                Console.WriteLine ($"{e}");
            }
        }

        public void LoadMap (string mapName)
        {
            CurrentMap = Map.Map.LoadFromFile ("pond.tmx");

            Size = new CGSize (CurrentMap.Width * 100, CurrentMap.Height * 50 + 300);
            AnchorPoint = new CGPoint (0.5, 1);
        }

        void SetCameraConstraints (SKCameraNode camera, SKSpriteNode sprite)
        {
            // Constrain the camera to the player
            var zeroRange = new SKRange (0, 0);
            SKConstraint playerConstraint = SKConstraint.CreateDistance (zeroRange, sprite);

            var scaledSize = new CGSize (Size.Width * camera.XScale, Size.Height * camera.YScale);
            var contentRect = mapNode.CalculateAccumulatedFrame ();
            contentRect = new CGRect (contentRect.X, contentRect.Y, contentRect.Width, contentRect.Height + 300);
                
            nfloat xInset = (nfloat)Math.Min ((scaledSize.Width / 2.0) + 10, contentRect.Width / 2.0);
            nfloat yInset = (nfloat)Math.Min ((scaledSize.Height / 2.0) + 10, contentRect.Height / 2.0);

            var insetRect = contentRect.Inset (xInset, yInset);

            var xRange = new SKRange (insetRect.GetMinX (), insetRect.GetMaxX ());
            var yRange = new SKRange (insetRect.GetMinY (), insetRect.GetMaxY ());

            var levelEdgeConstraint = SKConstraint.CreateRestriction (xRange, yRange);
            levelEdgeConstraint.ReferenceNode = this;

            camera.Constraints = new[] { playerConstraint, levelEdgeConstraint };
        }

        void SetupItemPositions ()
        {
            foreach (var item in ItemFactory.Items) {
                mapNode.AddItem (item);
            }
        }

        void SetupCharacterPositions ()
        {
            foreach (var character in CharacterFactory.Characters) {
                mapNode.AddCharacter (character);
            }
        }

        public override void DidMoveToView (SKView view)
        {
            cameraNode = new SKCameraNode ();
            Camera = cameraNode;
            Camera.XScale = 0.5f;
            Camera.YScale = 0.5f;
            AddChild (cameraNode);

            mapNode = new MapNode (CurrentMap);
            mapNode.Position = new CGPoint (mapNode.Position.X, mapNode.Position.Y + 100);
            AddChild (mapNode);

            mapNode.MapClicked += HandleTouchOnMap;
            mapNode.AddCharacter (Player);

            var comp = (CharacterSpriteComponent)Player.GetComponent (typeof(CharacterSpriteComponent));
            comp.Direction = Direction.North;
            comp.Walking = false;

            SetupItemPositions ();
            SetupCharacterPositions ();

            SetCameraConstraints (cameraNode, comp.Sprite);
        }

        void HandleTouchOnMap (object sender, MapClickedArgs args)
        {
            Map.Map.Position destination = args.ClickPosition;
            MoveComponent comp = (MoveComponent)Player.GetComponent (typeof(MoveComponent));
            comp.MoveEntity (destination, (bool success) => {
                if (!success) {
                    Console.WriteLine ("Can't do that");
                }
            });
        }

        public override void Update (double currentTime)
        {
            // Called before each frame is rendered
        }
    }
}

