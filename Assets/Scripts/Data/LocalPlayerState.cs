using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerState : MonoBehaviour
{
    private int _currentSlotIndex = 0;
    private List<ItemUI> _itemSlots;

    private void Awake()
    {
        _itemSlots = new List<ItemUI>(GetComponentsInChildren<ItemUI>());
    }

    public void SetActiveSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _itemSlots.Count) return;

        for (int i = 0; i < _itemSlots.Count; i++)
        {
            _itemSlots[i].SetActive(i == slotIndex);
        }

        _currentSlotIndex = slotIndex;
    }
}
