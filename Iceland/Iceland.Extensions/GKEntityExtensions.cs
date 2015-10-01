using System;
using GameplayKit;

namespace Iceland.Extensions
{
    public static class GKEntityExtensions
    {
        public static T GetComponent<T> (this GKEntity entity) where T : GKComponent
        {
            return (T)entity.GetComponent (typeof (T));
        }
    }
}

