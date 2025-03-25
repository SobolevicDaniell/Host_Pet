using Fusion;
using Leopotam.Ecs;
using UnityEngine;

namespace Game
{
    public class DebugSystem : IEcsRunSystem
    {
        private readonly NetworkRunner _runner;
        private readonly EcsFilter<PlayerComponent> _playerFilter = null;

        public DebugSystem(NetworkRunner runner)
        {
            _runner = runner;
        }

        public void Run()
        {
            if (!_runner.IsServer && !_runner.IsSharedModeMasterClient)
                return;

            Debug.ClearDeveloperConsole();


            foreach (var i in _playerFilter)
            {
                ref var player = ref _playerFilter.Get1(i);
                Debug.Log($"{player.playerRef.PlayerId}: {player.position}");
            }
        }
    }
}
