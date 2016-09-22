using System;
using GameplayKit;

using Iceland.Map;
using Iceland.Conversation;

namespace Iceland.Characters
{
    public class Entity : GKEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public EntityModel Model { get; set; }
    }
}

