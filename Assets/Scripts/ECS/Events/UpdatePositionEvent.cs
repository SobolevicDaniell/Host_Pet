using Fusion;
using UnityEngine;

namespace Game
{
    public struct UpdatePositionEvent
    {
        public PlayerRef playerRef;
        public Vector3 position;
        public Quaternion rotation;
    }
}