using Fusion;
using Leopotam.Ecs;
using UnityEngine;

namespace Game
{
    public class ServerInputSystem : IEcsRunSystem
    {
        private readonly NetworkRunner _runner;

        private readonly EcsFilter<PlayerComponent> _playerFilter = null;

        public ServerInputSystem(NetworkRunner runner)
        {
            _runner = runner;
        }

        public void Run()
        {
            if (!_runner.IsServer && !_runner.IsSharedModeMasterClient)
                return;

            foreach (var i in _playerFilter)
            {
                ref var player = ref _playerFilter.Get1(i);
                if (_runner.TryGetInputForPlayer<InputData>(player.playerRef, out var inputData))
                {
                    Debug.Log($"Player {player.playerRef.PlayerId}: H={inputData.movement.x}, V={inputData.movement.y}, Jump={inputData.buttons}");
                }
                else
                {
                    Debug.Log("Error");
                }
            }
        }
    }
}
