using System;

using GameplayKit;

namespace Iceland.Characters
{
    public interface IMenuAction
    {
        string Label { get; }
        void Activate (CharacterEntity playerEntity);
    }
}

