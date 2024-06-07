using UnityEngine;
using System.Collections.Generic;

public class BaseTree : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth = 20f;
    [SerializeField] private int _maxResourcesAmount = 4;
    [SerializeField] private int _minResourcesAmount = 3;
    [SerializeField] private ItemDataSO _woodData;

    private FlashSprite _flash;
    private float _resourceYOffset = -1f;
    private List<GameObject> _resourcesContained = new();
    private float _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
        InitializeResourceItems();
        _flash = GetComponent<FlashSprite>();
    }

    public void TakeDamage(DamageStruct damageStruct)
    {
        _flash.QuickFlash();

        if (damageStruct.damage > _currentHealth)
            _currentHealth = 0;
        else
            _currentHealth -= damageStruct.damage;

        if (_currentHealth == 0)
            Die();
    }

    private void Die()
    {
        DropResources();

        transform.gameObject.SetActive(false);
    }

    private void DropResources()
    {
        foreach(GameObject item in _resourcesContained)
        {
            item.gameObject.SetActive(true);
            item.gameObject.transform.parent = null;
        }

        _resourcesContained.Clear();
    }

    private void InitializeResourceItems()
    {
        int randomResourceCount = Random.Range(_minResourcesAmount, _maxResourcesAmount + 1);

        for (int i = 0; i < randomResourceCount; i++)
        {
            GameObject item = ItemFactory.Instance.CreateItem(_woodData);

            _resourcesContained.Add(item);

            item.SetActive(false);

            Vector3 position = new(transform.position.x, transform.position.y + _resourceYOffset, 0);
            item.transform.position = position;

            item.transform.parent = transform;

            item.AddComponent<MoveToRandomPositionAround>();
        }
    }
}