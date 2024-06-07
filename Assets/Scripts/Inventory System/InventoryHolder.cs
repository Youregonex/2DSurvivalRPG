using UnityEngine;

[System.Serializable]
public class InventoryHolder : MonoBehaviour
{
    [SerializeField] protected int _primaryInventorySize;
    [SerializeField] protected InventorySystem _primaryInventorySystem;

    public InventorySystem PrimaryInventorySystem => _primaryInventorySystem;

    protected virtual void Awake()
    {
        _primaryInventorySystem = new InventorySystem(_primaryInventorySize);
    }
}
