using UnityEngine;

namespace Redpenguin.Core.StaticData
{
    public abstract class ScriptableObject<T> : ScriptableObject
    {
        public T Data;
    }
}