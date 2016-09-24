using System;

namespace Iceland.Characters
{
    public class Professor : Character
    {
        public Professor ()
        {
            Model = new EntityModel () {
                StartPosition = new Map.Map.Position { Row = 0, Column = 0 },
            };

            AddComponent (new CharacterSpriteComponent ("professor_walk_cycle_no_hat", this));
            AddComponent (new MoveComponent ());
            AddComponent (new InventoryComponent ());

            Name = "The Professor";
        }
    }
}

