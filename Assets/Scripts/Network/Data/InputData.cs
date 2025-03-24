using Fusion;
using UnityEngine;

public struct InputData : INetworkInput
{
    //public float horizontal;
    //public float vertical;
    //public bool Jump;

    public NetworkButtons buttons;
    public Vector2 movement;
}