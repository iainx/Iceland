using System;

using Foundation;
using SpriteKit;
using UIKit;

namespace Iceland
{
    public partial class GameViewController : UIViewController
    {
        EntityClickHandler clickHandler;
        ConversationHandler conversationHandler;
        LookHandler lookHandler;
        CollectHandler collectHandler;

        UIButton inventoryButton;

        public static GameScene CurrentScene { get; private set; }

        UIViewController menuController;

        public GameViewController (IntPtr handle) : base (handle)
        {
            clickHandler = new EntityClickHandler (this);
            conversationHandler = new ConversationHandler (this);
            lookHandler = new LookHandler (this);
            collectHandler = new CollectHandler (this);
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            // Configure the view.
            var skView = (SKView)View;
            skView.ShowsFPS = true;
            skView.ShowsNodeCount = true;
            /* Sprite Kit applies additional optimizations to improve rendering performance */
            skView.IgnoresSiblingOrder = true;

            // Create and configure the scene.
            var scene = SKNode.FromFile<GameScene> ("GameScene");
            scene.ScaleMode = SKSceneScaleMode.AspectFill;
            CurrentScene = scene;

            /*
            var redbox = new SKSpriteNode (UIColor.Red, new CoreGraphics.CGSize (100, 100));
            var bluebox = new SKSpriteNode (UIColor.Blue, new CoreGraphics.CGSize (50, 50));
            var greenbox = new SKSpriteNode (UIColor.Green, new CoreGraphics.CGSize (25, 25));

            redbox.AnchorPoint = new CoreGraphics.CGPoint (0, 0);
            redbox.Position = new CoreGraphics.CGPoint (0, 0);
            redbox.Add (bluebox);

            bluebox.Add (greenbox);
            bluebox.Position = CoreGraphics.CGPoint.Empty;
            bluebox.AnchorPoint = CoreGraphics.CGPoint.Empty;

            greenbox.Position = CoreGraphics.CGPoint.Empty;
            greenbox.AnchorPoint = CoreGraphics.CGPoint.Empty;

            scene.Add (redbox);
            */
            // Present the scene.
            skView.PresentScene (scene);

            inventoryButton = new UIButton (UIButtonType.InfoDark);
            inventoryButton.TranslatesAutoresizingMaskIntoConstraints = false;
            View.AddSubview (inventoryButton);

            inventoryButton.TouchUpInside += (sender, e) => {
                var ivc = new InventoryViewController { Manager = CurrentScene.InvManager };
                ivc.ItemActivated += (s, args) => {
                    DismissViewController (false, null);
                    MenuDisplayController.DisplayMenuForItem (args.Item, this, clickHandler);
                };

                PresentViewController (ivc, false, null);
            };

            var viewsDict = new NSDictionary ("inventory", inventoryButton);
            var constraints = NSLayoutConstraint.FromVisualFormat ("|-20-[inventory]", NSLayoutFormatOptions.AlignAllTop, null, viewsDict);
            skView.AddConstraints (constraints);

            constraints = NSLayoutConstraint.FromVisualFormat ("V:|-20-[inventory]", NSLayoutFormatOptions.AlignAllTop, null, viewsDict);
            skView.AddConstraints (constraints);
        }

        public override bool ShouldAutorotate ()
        {
            return true;
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
        {
            return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone ? UIInterfaceOrientationMask.AllButUpsideDown : UIInterfaceOrientationMask.All;
        }

        public override void DidReceiveMemoryWarning ()
        {
            base.DidReceiveMemoryWarning ();
            // Release any cached data, images, etc that aren't in use.
        }

        public override bool PrefersStatusBarHidden ()
        {
            return true;
        }
    }
}

