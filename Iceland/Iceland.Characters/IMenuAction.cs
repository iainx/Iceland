using System;

namespace Iceland.Characters
{
    public interface IMenuAction
    {
        string Label { get; }
        void Activate ();
    }
}

