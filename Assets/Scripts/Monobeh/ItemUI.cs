using UnityEngine;

public class ItemUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _activeImage;
    private bool _isActive;

    public void SetActive(bool value)
    {
        _isActive = value;
        _activeImage.SetActive(_isActive);
    }
}
