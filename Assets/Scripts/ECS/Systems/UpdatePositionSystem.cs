using Fusion;
using Game;
using Leopotam.Ecs;
using UnityEngine;

namespace Game
{
    public class UpdatePositionSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly NetworkRunner _runner;

        private PlayerRef _eventPlayerRef;
        private PlayerRef _playerRef;
        private Vector3 _position;
        private Quaternion _rotation;

        private readonly EcsFilter<UpdatePositionEvent> _eventFilter = null;
        private readonly EcsFilter<PlayerComponent> _playerFilter = null;

        public UpdatePositionSystem(EcsWorld world, NetworkRunner runner)
        {
            _world = world;
            _runner = runner;
        }
        public void Run()
        {
            if (!_runner.IsServer && !_runner.IsSharedModeMasterClient)
                return;

            foreach (var i in _eventFilter)
            {
                ref var positionEvent = ref _eventFilter.Get1(i);
                _eventPlayerRef = positionEvent.playerRef;
                _position = positionEvent.position;
                _rotation = positionEvent.rotation;

                foreach (var j in _playerFilter)
                {
                    ref var players = ref _playerFilter.Get1(j);
                    _playerRef = players.playerRef;

                    if (_eventPlayerRef == _playerRef)
                    {
                        players.position = _position;
                        players.rotation = _rotation;
                        _eventFilter.GetEntity(i).Destroy();
                    }
                }
                
            }
        }  
    }
}