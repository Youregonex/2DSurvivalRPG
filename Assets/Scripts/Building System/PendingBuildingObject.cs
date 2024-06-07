using UnityEngine;
using System.Collections.Generic;

public class PendingBuildingObject : MonoBehaviour
{
    [SerializeField] private Color _canBuildColor;
    [SerializeField] private Color _cantBuildColor;

    public bool canBeBuilt { get; private set; } = false;
    public bool inBuildingRange = true;

    [SerializeField] private List<Transform> _collisions = new();
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DisplayPendingBuildingObject(BuildingItemDataSO buildingItem)
    {
        UpdateBuildingItemData(buildingItem);

        gameObject.SetActive(true);
    }

    public void HidePendingBuildingObject()
    {
        gameObject.SetActive(false);
    }

    private void UpdateBuildingItemData(BuildingItemDataSO buildingItem)
    {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = buildingItem.Icon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _collisions.Add(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _collisions.Remove(collision.transform);
    }

    private void Update()
    {
        if(_collisions.Count == 0 && inBuildingRange)
        {
            canBeBuilt = true;
            _spriteRenderer.color = _canBuildColor;
        }
        else
        {
            canBeBuilt = false;
            _spriteRenderer.color = _cantBuildColor;
        }
    }
}