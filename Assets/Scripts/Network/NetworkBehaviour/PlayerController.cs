using UnityEngine;
using Fusion;
using Leopotam.Ecs;

namespace Game
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private Transform _cameraHolder;
        [SerializeField] private PlayerSO _playerSO;
        [SerializeField] private LocalPlayerState _localPlayerState;

        private CharacterController _characterController;
        private float _cameraPitch = 0f;
        private float _verticalVelocity = 0f;
        private bool _isGrounded;
        private EcsWorld _world;
        private PlayerRef _playerRef;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            Startup startup = FindObjectOfType<Startup>();
            _world = startup.world;

            Cursor.lockState = CursorLockMode.Locked;
            _playerRef = Object.HasInputAuthority ? Runner.LocalPlayer : Object.InputAuthority;
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput<InputData>(out var input))
            {
                ProcessMouseLook(input);
                ProcessMovement(input);
                ProcessJump(input);
                UpdatePositionComponent();
            }
        }

        private void Update()
        {
            if (HasInputAuthority)
            {
                HandleSlotSwitching();
            }
        }

        private void HandleSlotSwitching()
        {
            for (int i = 0; i < 10; i++)
            {
                if (Input.GetKeyDown(i == 9 ? KeyCode.Alpha0 : KeyCode.Alpha1 + i))
                {
                    _localPlayerState.SetActiveSlot(i);
                    break;
                }
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
            _isGrounded = _characterController.isGrounded;
            Vector3 direction = new Vector3(input.movement.x, 0, input.movement.y);
            direction = transform.TransformDirection(direction);
            _characterController.Move(direction * _playerSO.PlayerSpeed * Runner.DeltaTime);
        }

        private void ProcessJump(InputData input)
        {
            if (_isGrounded)
            {
                _verticalVelocity = -1f;
                if (input.jump)
                {
                    _verticalVelocity = Mathf.Sqrt(2f * _playerSO.JumpHeight * _playerSO.Gravity);
                }
            }
            else
            {
                _verticalVelocity -= _playerSO.Gravity * Runner.DeltaTime;
            }
            _characterController.Move(Vector3.up * _verticalVelocity * Runner.DeltaTime);
        }

        private void UpdatePositionComponent()
        {
            if (_world != null)
            {
                EcsEntity entity = _world.NewEntity();
                ref var positionEvent = ref entity.Get<UpdatePositionEvent>();
                positionEvent.playerRef = _playerRef;
                positionEvent.position = transform.position;
                positionEvent.rotation = transform.rotation;
            }
        }
    }
}
