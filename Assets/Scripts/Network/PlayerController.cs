using UnityEngine;
using Fusion;
using Leopotam.Ecs;
using Game;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float _speed = 5f;

    private CharacterController _characterController;
    private EcsWorld _world;

    public PlayerRef playerRef;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Startup _startup = FindObjectOfType<Startup>();
        _world = _startup.world;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput<InputData>(out var input))
        {
            ProcessInput(input);
            UpdatePositionComponent();
        }
        //if (Object.HasInputAuthority && GetInput<InputData>(out var input))
        //{
        //    ProcessInput(input);
        //}
        //if (Object.HasStateAuthority)
        //{
        //    UpdatePositionComponent();
        //}
    }

    private void ProcessInput(InputData input)
    {
        Vector3 direction = new Vector3(input.movement.x, 0, input.movement.y);
        _characterController.Move(direction * _speed * Runner.DeltaTime);

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void UpdatePositionComponent()
    {
        if (_world != null)
        {
            //EcsEntity entity = _world.NewEntity();
            //ref var playerPositionEvent = ref entity.Get<UpdatePositionEvent>();
            ////playerPositionEvent.playerRef = Object.HasStateAuthority ? Runner.LocalPlayer : default;
            //playerPositionEvent.playerRef = Runner.LocalPlayer;
            //playerPositionEvent.position = transform.position;
            //playerPositionEvent.rotation = transform.rotation;

            ////Debug.Log($"Client sent position: {playerPositionEvent.position} for {playerPositionEvent.playerRef}");


            EcsEntity entity = _world.NewEntity();
            ref var positionEvent = ref entity.Get<UpdatePositionEvent>();
            //positionEvent.playerRef = Object.HasInputAuthority ? Object.InputAuthority : Runner.LocalPlayer;
            positionEvent.playerRef = playerRef;
            positionEvent.position = transform.position;
            positionEvent.rotation = transform.rotation;

            //Debug.Log($"[UpdatePositionComponent] Sent position: {positionEvent.position} for PlayerRef: {positionEvent.playerRef.PlayerId}");
        
        }
    }
}
