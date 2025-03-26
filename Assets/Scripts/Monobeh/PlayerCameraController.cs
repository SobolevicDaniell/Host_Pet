using Fusion;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private NetworkObject _networkObject;

    private void Awake()
    {
        _networkObject = GetComponentInParent<NetworkObject>();
    }

    private void Start()
    {
        if (_networkObject.HasInputAuthority)
        {
            _camera.enabled = true;
            _camera.gameObject.SetActive(true);
        }
        else
        {
            _camera.enabled = false;
            _camera.gameObject.SetActive(false);
        }
    }
}
