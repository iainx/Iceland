using System;

using GameplayKit;

namespace Iceland.Characters
{
    public interface IMenuAction
    {
        string Label { get; }
        bool OnlyIfCollected { get; }
        bool OnlyIfDropped { get; }

        void Activate (Entity playerEntity, Entity otherEntity);
    }
}

