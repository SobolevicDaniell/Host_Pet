using UnityEngine;
using Fusion;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float _speed = 5f;

    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        // Обработка ввода
        if (GetInput<InputData>(out var input))
        {
            ProcessInput(input);
        }
    }

    private void ProcessInput(InputData input)
    {
        // Движение
        Vector3 direction = new Vector3(input.movement.x, 0, input.movement.y);
        _characterController.Move(direction * _speed * Runner.DeltaTime);

        // Вращение игрока
        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
