using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UniqueID))]
public class Item : MonoBehaviour
{
    [field: SerializeField] public ItemDataSO ItemData { get; private set; }
    [field: SerializeField] public int Quantity { get; private set; }

    private SpriteRenderer _spriteRenderer;
    private float _uninteractableTime = .7f;
    private CircleCollider2D _itemCollider;
    private IEnumerator _currentCoroutine;

    private string _id;

    public string ID => _id;

    private void Awake()
    {
        _id = GetComponent<UniqueID>().ID;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _itemCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        _spriteRenderer.sprite = ItemData.Icon;

        _currentCoroutine = DisableCollider();

        StartCoroutine(_currentCoroutine);
    }

    public void DestroyItem()
    {
        Destroy(transform.parent.gameObject);
    }

    public void ChangeQuantity(int newQuantity) => Quantity = newQuantity;

    public void SetItemData(ItemDataSO item, int quantity = 1)
    {
        ItemData = item;
        Quantity = quantity;
    }

    private IEnumerator DisableCollider()
    {
        _itemCollider.enabled = false;

        yield return new WaitForSeconds(_uninteractableTime);

        _itemCollider.enabled = true;
    }
}