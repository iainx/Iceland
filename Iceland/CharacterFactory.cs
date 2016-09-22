using System;
using System.Collections.Generic;
using System.IO;

using Iceland.Characters;
using Newtonsoft.Json;

namespace Iceland
{
    public static class CharacterFactory
    {
        static Dictionary<string, Entity> characters = new Dictionary<string, Entity> ();
        public static IEnumerable<Entity> Characters {
            get { return characters.Values; }
        }

        public static Entity CreateCharacter (EntityModel model)
        {
            var character = new Entity {
                Name = model.Name,
            };

            character.AddComponent (new CharacterSpriteComponent (model.TextureName, character));

            bool needsClickableComponent = false;
            if (model.LookCommand != null) {
                character.AddComponent (new LookableComponent ());
                needsClickableComponent = true;
            }

            if (model.TalkCommand != null) {
                character.AddComponent (new TalkableComponent ());
                needsClickableComponent = true;
            }

            if (needsClickableComponent) {
                character.AddComponent (new ClickableComponent ());
            }
            characters [character.Name] = character;

            character.Model = model;

            return character;
        }

        public static Entity CreateCharacter (string filename)
        {
            string contents = File.ReadAllText ("Characters/" + filename);
            var model = JsonConvert.DeserializeObject<EntityModel> (contents);

            return CreateCharacter (model);
        }
    }
}

