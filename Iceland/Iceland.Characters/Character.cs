using System;
using SpriteKit;

using Iceland.Extensions;

namespace Iceland.Characters
{
    public class Character : Entity
    {
        public void PerformAction (IMenuAction action, Entity subject, Entity obj)
        {
            MoveComponent comp = this.GetComponent<MoveComponent> ();
            comp.MoveEntity (obj.Model.ActionPosition, (bool success) => {
                if (success == false) {
                    Console.WriteLine ("Can't do that");
                    return;
                }

                action.Activate (GameViewController.CurrentScene.Player, obj);
            });
        }
    }
}

