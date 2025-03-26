using Fusion;
using Leopotam.Ecs;
using UnityEngine;

namespace Game
{
    public class PlayerSpawnSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly NetworkRunner _runner;

        private readonly PlayerSO _playerSO;


        private readonly EcsFilter<PlayerSpawnEvent> playerSpawnEventFilter = null;

        public PlayerSpawnSystem(EcsWorld world, NetworkRunner runner, PlayerSO playerSO)
        {
            _world = world;
            _runner = runner;
            _playerSO = playerSO;
        }

        public void Run()
        {
            if (!_runner.IsServer && !_runner.IsSharedModeMasterClient)
                return;

            foreach (var i in playerSpawnEventFilter)
            {
                ref var spawnEvent = ref playerSpawnEventFilter.Get1(i);
                if (!spawnEvent.isSpawned)
                {
                    SpawnPlayer(spawnEvent.playerRef);
                    spawnEvent.isSpawned = true;
                    playerSpawnEventFilter.GetEntity(i).Destroy();
                }
            }
        }

        private void SpawnPlayer(PlayerRef player)
        {
            Vector3 spawnPosition = new Vector3(0, 0, 0 + player.PlayerId * 2);

            //Debug.Log($"Spawning player {player.PlayerId} at position {spawnPosition}");

            NetworkObject playerObject = _runner.Spawn(_playerSO.PlayerPrefab, spawnPosition, Quaternion.identity, player);

            _runner.SetPlayerObject(player, playerObject);
            PlayerController controller = playerObject.GetComponent<PlayerController>();

            var entity = _world.NewEntity();
            ref var playerComponent = ref entity.Get<PlayerComponent>();
            playerComponent.playerRef = player;
            playerComponent.position = spawnPosition;
            playerComponent.playerGameObject = playerObject.gameObject;
        }
    }
}