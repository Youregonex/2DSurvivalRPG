using UnityEngine;

public abstract class Building : MonoBehaviour
{
    [field: SerializeField] public BuildingItemDataSO BuildingData { get; private set; }

    protected float _durability;
    protected SpriteRenderer _spriteRenderer;

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetBuildingData(BuildingItemDataSO buildingData)
    {
        _spriteRenderer.sprite = buildingData.Icon;
        _durability = buildingData.durability;
    }
}
