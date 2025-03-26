using Fusion;
using UnityEngine;

public struct InputData : INetworkInput
{
    public NetworkButtons buttons;
    public Vector2 movement;
    public float mouseX;
    public float mouseY;
}
