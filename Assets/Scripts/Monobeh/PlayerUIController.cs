using UnityEngine;
using Fusion;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private ItemUI[] _slotUIElements;

    private NetworkObject _networkObject;
    private int _currentSlotIndex = 0;

    private void Awake()
    {
        _networkObject = GetComponentInParent<NetworkObject>();
    }

    private void Start()
    {
        bool isLocal = _networkObject.HasInputAuthority;
        _canvas.enabled = isLocal;
        _canvas.gameObject.SetActive(isLocal);

        UpdateActiveSlot(_currentSlotIndex);
    }

    public void UpdateActiveSlot(int newSlotIndex)
    {
        
        if (newSlotIndex < 0 || newSlotIndex >= _slotUIElements.Length) return;

        for (int i = 0; i < _slotUIElements.Length; i++)
        {
            _slotUIElements[i].SetActive(i == newSlotIndex);
        }

        _currentSlotIndex = newSlotIndex;
    }
}
