using Fusion;
using Leopotam.Ecs;
using UnityEngine;

namespace Game
{
    public class PlayerLeftSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly NetworkRunner _runner;

        private readonly EcsFilter<PlayerLeftEvent> _playerLeftEventFilter = null;
        private readonly EcsFilter<PlayerComponent> _playerComponentFilter = null;

        public PlayerLeftSystem(EcsWorld world, NetworkRunner runner)
        {
            _world = world;
            _runner = runner;
        }

        public void Run()
        {
            if (!_runner.IsServer && !_runner.IsSharedModeMasterClient)
                return;

            foreach (var i in _playerLeftEventFilter)
            {
                ref var leftEvent = ref _playerLeftEventFilter.Get1(i);
                if (!leftEvent.isLeft)
                {
                    DeletePlayer(leftEvent.playerRef);
                    leftEvent.isLeft = true;

                    _playerLeftEventFilter.GetEntity(i).Destroy();
                }
            }
        }

        private void DeletePlayer(PlayerRef player)
        {
            foreach (var i in _playerComponentFilter)
            {
                ref var playerComponent = ref _playerComponentFilter.Get1(i);

                if (playerComponent.playerRef == player)
                {
                    if (playerComponent.playerGameObject != null)
                    {
                        Debug.Log($"Removing player object: {player.PlayerId}");
                        _runner.Despawn(playerComponent.playerGameObject.GetComponent<NetworkObject>());
                        Object.Destroy(playerComponent.playerGameObject);
                    }

                    _playerComponentFilter.GetEntity(i).Destroy();
                    break;
                }
            }
        }
    }
}
