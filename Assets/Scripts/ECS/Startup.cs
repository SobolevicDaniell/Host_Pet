using Fusion;
using Leopotam.Ecs;
using UnityEngine;

namespace Game
{
    public class Startup : MonoBehaviour
    {
        private EcsWorld _world;
        public EcsWorld world => _world;
        private EcsSystems _systems;

        [SerializeField] private GameObject _playerPrefab;

        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            FusionStartup fusionStartup = FindObjectOfType<FusionStartup>();
            NetworkRunner runner = fusionStartup?.runner;

            _systems
                .Add(new PlayerSpawnSystem(_world, runner, _playerPrefab))
                .Add(new PlayerLeftSystem(_world, runner))
                .Add(new ServerInputSystem(runner));

            _systems.Init();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            _systems?.Destroy();
            _world?.Destroy();
        }
    }
}
