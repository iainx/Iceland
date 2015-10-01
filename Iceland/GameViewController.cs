using System;

using SpriteKit;
using UIKit;

namespace Iceland
{
    public partial class GameViewController : UIViewController
    {
        EntityClickHandler clickHandler;

        public GameViewController (IntPtr handle) : base (handle)
        {
            clickHandler = new EntityClickHandler ();
            clickHandler.DisplayMenu += (sender, e) => {
                var alert = UIAlertController.Create ("", "", UIAlertControllerStyle.ActionSheet);

                foreach (var i in e.menuItems) {
                    var localItem = i;
                    var a = UIAlertAction.Create (i.Label, 0, (obj) => localItem.OnActivated ());
                    alert.AddAction (a);
                }

                var cancelItem = UIAlertAction.Create ("Cancel", UIAlertActionStyle.Cancel, null);
                alert.AddAction (cancelItem);
                PresentViewController (alert, true, null);
            };
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            // Code to start the Xamarin Test Cloud Agent
            #if ENABLE_TEST_CLOUD
            Xamarin.Calabash.Start();
            #endif

            // Configure the view.
            var skView = (SKView)View;
            skView.ShowsFPS = true;
            skView.ShowsNodeCount = true;
            /* Sprite Kit applies additional optimizations to improve rendering performance */
            skView.IgnoresSiblingOrder = true;

            // Create and configure the scene.
            var scene = SKNode.FromFile<GameScene> ("GameScene");
            scene.ScaleMode = SKSceneScaleMode.AspectFill;

            // Present the scene.
            skView.PresentScene (scene);
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

