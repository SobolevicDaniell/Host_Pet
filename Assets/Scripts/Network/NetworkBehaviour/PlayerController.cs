using UnityEngine;
using Fusion;
using Leopotam.Ecs;
using Game;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Transform _cameraHolder;
    [SerializeField] private PlayerSO _playerSO;

    private CharacterController _characterController;
    private EcsWorld _world;

    private float _cameraPitch = 0f;

    public PlayerRef playerRef;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Startup _startup = FindObjectOfType<Startup>();
        _world = _startup.world;

        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput<InputData>(out var input))
        {
            ProcessMouseLook(input);
            ProcessMovement(input);
            UpdatePositionComponent();
        }
    }

    private void ProcessMouseLook(InputData input)
    {
        _cameraPitch -= input.mouseY * _playerSO.MouseSensitivity;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -90f, 90f);
        _cameraHolder.localRotation = Quaternion.Euler(_cameraPitch, 0, 0);

        transform.Rotate(0, input.mouseX * _playerSO.MouseSensitivity, 0);
    }

    private void ProcessMovement(InputData input)
    {
        Vector3 direction = new Vector3(input.movement.x, 0, input.movement.y);
        direction = transform.TransformDirection(direction);

        _characterController.Move(direction * _playerSO.PlayerSpeed * Runner.DeltaTime);
    }

    private void UpdatePositionComponent()
    {
        if (_world != null)
        {
            EcsEntity entity = _world.NewEntity();
            ref var positionEvent = ref entity.Get<UpdatePositionEvent>();
            positionEvent.playerRef = playerRef;
            positionEvent.position = transform.position;
            positionEvent.rotation = transform.rotation;
        }
    }
}
