using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using Leopotam.Ecs;
using UnityEngine;

namespace Game
{
    public class PlayerSpawnSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly NetworkRunner _runner;
        private readonly GameObject _playerPrefab;

        private readonly EcsFilter<PlayerSpawnEvent> playerSpawnEventFilter = null;

        public PlayerSpawnSystem(EcsWorld world, NetworkRunner runner, GameObject playerPrefab)
        {
            _world = world;
            _runner = runner;
            _playerPrefab = playerPrefab;
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

            Debug.Log($"Spawning player {player.PlayerId} at position {spawnPosition}");

            NetworkObject playerObject = _runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);

            _runner.SetPlayerObject(player, playerObject);

            var entity = _world.NewEntity();
            ref var playerComponent = ref entity.Get<PlayerComponent>();
            playerComponent.playerRef = player;
            playerComponent.position = spawnPosition;
            playerComponent.playerGameObject = playerObject.gameObject;
        }

    }
}