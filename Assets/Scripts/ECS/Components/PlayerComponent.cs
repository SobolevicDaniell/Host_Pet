using Fusion;
using UnityEngine;
namespace Game
{
    public struct PlayerComponent
    {
        public PlayerRef playerRef;
        public Vector3 position;
        public Quaternion rotation;
        public GameObject playerGameObject;
    }
}