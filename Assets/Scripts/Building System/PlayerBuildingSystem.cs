using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerBuildingSystem : MonoBehaviour, IEventListener
{
    [SerializeField] private PendingBuildingObject _pendingBuildingObject;
    [SerializeField] private float _buildingRange = 5f;
    [SerializeField] private List<Building> _playerBuildings = new();

    private PlayerInventoryHolder _playerInventory;
    private InventorySlot _currentSlot;

    private Camera _mainCamera;
    private bool _buildingEnabled = false;
    private BuildingItemDataSO _currentBuildingItemData;

    private Vector3 _mousePosition;
    private Vector3 _worldMousePosition;
    Vector3 _worldMousePositionRound;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _playerInventory = GetComponent<PlayerInventoryHolder>();

        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    public void EnableBuildingSystem(InventorySlot buildingSlot)
    {
        _buildingEnabled = true;

        BuildingItemDataSO buildingItemSO = (BuildingItemDataSO)buildingSlot.Item;
        _pendingBuildingObject.DisplayPendingBuildingObject(buildingItemSO);

        _currentSlot = buildingSlot;
        _currentBuildingItemData = buildingItemSO;
    }

    public void DisableBuildingSystem()
    {
        _buildingEnabled = false;
        _pendingBuildingObject.HidePendingBuildingObject();
    }

    private void Update()
    {
        if(_buildingEnabled)
        {
            _pendingBuildingObject.transform.position = GetGridMousePosition();

            if (GetSqrLength() < _buildingRange * _buildingRange)
                _pendingBuildingObject.inBuildingRange = true;
            else
                _pendingBuildingObject.inBuildingRange = false;
        }
    }

    private Vector3 GetGridMousePosition()
    {
        _mousePosition = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0);
        _worldMousePosition = _mainCamera.ScreenToWorldPoint(new Vector3(_mousePosition.x, _mousePosition.y, 0));

        _worldMousePositionRound = new Vector3(Mathf.Round(_worldMousePosition.x),
                                               Mathf.Round(_worldMousePosition.y),
                                               0);
        return _worldMousePositionRound;
    }

    private float GetSqrLength()
    {
        Vector3 offset = _pendingBuildingObject.transform.position - transform.position;
        float sqrLength = offset.sqrMagnitude;

        return sqrLength;
    }

    private void TryBuildingObject()
    {
        if(_buildingEnabled && _pendingBuildingObject.canBeBuilt)
        {
            Building building = BuildingFactory.Instance.CreateBuildingAtPosition(_currentBuildingItemData, _worldMousePositionRound);
            _playerBuildings.Add(building);

            _playerInventory.DecreaseSlotQuantity(_currentSlot);
        }
    }

    public void SubscribeToEvents()
    {
        EventBus.Instance.OnLMBKeyPressed += TryBuildingObject;
    }

    public void UnsubscribeFromEvents()
    {
        EventBus.Instance.OnLMBKeyPressed -= TryBuildingObject;
    }
}
