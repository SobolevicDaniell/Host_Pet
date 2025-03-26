using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerSO", menuName = "Game/Player")]
public class PlayerSO : ScriptableObject
{
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float jumpVelocity = 10f;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private float mouseSensitivity;

    public float PlayerSpeed => playerSpeed;
    public float JumpVelocity => jumpVelocity;
    public GameObject PlayerPrefab => playerPrefab;

    public float MouseSensitivity => mouseSensitivity;
}
