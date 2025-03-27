using Fusion;
using Fusion.Sockets;
using UnityEngine;
using System.Collections.Generic;
using Leopotam.Ecs;

namespace Game
{
    public class CallbacksController : MonoBehaviour, INetworkRunnerCallbacks
    {
        private EcsWorld _world;

        private void Start()
        {
            Startup startup = FindObjectOfType<Startup>();
            _world = startup.world;

            Cursor.lockState = CursorLockMode.Locked;
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (_world != null)
            {
                EcsEntity entity = _world.NewEntity();
                ref var spawnEvent = ref entity.Get<PlayerSpawnEvent>();
                spawnEvent.playerRef = player;
                spawnEvent.isSpawned = false;
                Debug.Log("Player joined: " + player.PlayerId);
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log("Player left: " + player.PlayerId);
            if (_world != null)
            {
                EcsEntity entity = _world.NewEntity();
                ref var leftEvent = ref entity.Get<PlayerLeftEvent>();
                leftEvent.playerRef = player;
                leftEvent.isLeft = false;
            }
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var inputData = new InputData
            {
                movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")),
                mouseX = Input.GetAxis("Mouse X"),
                mouseY = Input.GetAxis("Mouse Y"),
                jump = Input.GetKey(KeyCode.Space)
            };

            input.Set(inputData);
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnUserSimulationMessage(NetworkRunner runner, Fusion.SimulationMessagePtr message) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, System.ArraySegment<byte> data) { }
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnDisconnectedFromServer(NetworkRunner runner) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    }
}
