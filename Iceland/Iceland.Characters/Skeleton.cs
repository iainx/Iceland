using System;
using GameplayKit;

using Iceland.Conversation;

namespace Iceland.Characters
{
    public class Skeleton : CharacterEntity
    {
        public Skeleton ()
        {
            Name = "Death";
            Description = "A cold looking skeleton";
            AddComponent (new CharacterSpriteComponent ("skeleton_walk_cycle", this));
            AddComponent (new ClickableComponent ());

            var talkableComp = new TalkableComponent ();
            var details = new Details ();
            details.Introduction = "Hello";

            var item = new Item ();
            item.Text = "What do you want?";
            details.Items.Add (item);

            var response = new Item.Response ();
            response.Text = "Who are you?";
            response.NextItem = 1;
            item.Responses.Add (response);

            response = new Item.Response ();
            response.Text = "Nothing";
            item.Responses.Add (response);

            item = new Item ();
            item.Text = "I am Death!";
            details.Items.Add (item);

            response = new Item.Response ();
            response.Text = "Eeek!";
            item.Responses.Add (response);

            talkableComp.Conversations.Add (details);

            AddComponent (talkableComp);
            AddComponent (new LookableComponent ());
        }
    }
}

