using System;
using System.Collections.Generic;

using SpriteKit;

using Newtonsoft.Json;

using Iceland.Characters;
using System.IO;

namespace Iceland
{
    public static class ItemFactory
    {
        static Dictionary<string, Entity> items = new Dictionary<string, Entity> ();
        public static IEnumerable<Entity> Items {
            get { return items.Values; }
        }

        static SKTextureAtlas atlas = null;

        public static Entity CreateItem (EntityModel model)
        {
            var item = new Entity {
                Name = model.Name,
                Id = model.Id,
                Model = model
            };

            if (atlas == null) {
                atlas = SKTextureAtlas.FromName ("Items");
            }
            var spriteComp = new ItemSpriteComponent (atlas.TextureNamed (model.TextureName), item);
            item.AddComponent (spriteComp);

            var clickableComp = new ClickableComponent ();
            item.AddComponent (clickableComp);

            var lookableComp = new LookableComponent ();
            item.AddComponent (lookableComp);

            var collectableComp = new CollectibleComponent ();
            item.AddComponent (collectableComp);

            var giveableComp = new GiveableComponent ();
            item.AddComponent (giveableComp);

            var useableComp = new UseableComponent ();
            item.AddComponent (useableComp);

            items [item.Id] = item;

            return item;
        }

        public static void CreateItem (string itemFilename)
        {
            try {
                var contents = File.ReadAllText ("Items/" + itemFilename);
                var model = JsonConvert.DeserializeObject<EntityModel> (contents);

                CreateItem (model);
            } catch (Exception e) {
                Console.WriteLine ($"exception: {e}");
            }
        }

        public static Entity GetItemNamed (string id)
        {
            Entity item;

            items.TryGetValue (id, out item);
            return item;
        }
    }
}

