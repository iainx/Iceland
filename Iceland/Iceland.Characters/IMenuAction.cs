using System;

using GameplayKit;

namespace Iceland.Characters
{
    public interface IMenuAction
    {
        string Label { get; }
        bool OnlyIfCollected { get; }
        bool OnlyIfDropped { get; }

        int NumberOfEntitiesNeeded { get; }
        void Activate (Entity playerEntity, Entity otherEntity);
    }
}

