using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public sealed class FusionStartup : MonoBehaviour
    {
        private NetworkRunner _runner;
        public NetworkRunner runner => _runner;

        [SerializeField]
        private string _sessionName = "Session1";

        private void Start()
        {
            StartGame(GameMode.AutoHostOrClient);
        }

        async void StartGame(GameMode mode)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;

            var joinController = GetComponent<CallbacksController>();
            if (joinController != null)
            {
                _runner.AddCallbacks(joinController);
            }

            var scene = SceneManager.GetActiveScene();
            var sceneRef = SceneRef.FromIndex(scene.buildIndex);
            var sceneInfo = new NetworkSceneInfo();
            if (sceneRef.IsValid)
            {
                sceneInfo.AddSceneRef(sceneRef, LoadSceneMode.Additive);
            }

            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = _sessionName,
                Scene = GetScene(),
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }

        private SceneRef GetScene()
        {
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            return SceneRef.FromIndex(buildIndex);
        }
    }
}
