using UnityEngine;

public class PlayerSelctedItem : MonoBehaviour, IEventListener
{
    [SerializeField] private InventorySlot _currentlySelectedSlot;

    private PlayerBuildingSystem _playerBuildingSystem;
    private PlayerState _playerState;


    private void Awake()
    {
        _currentlySelectedSlot = null;
        _playerState = GetComponent<PlayerState>();
        _playerBuildingSystem = GetComponent<PlayerBuildingSystem>();

        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void UpdatePlayerItem(InventorySlot newSlot)
    {
        _currentlySelectedSlot = newSlot;

        _playerState.RemovePlayerState(PlayerStateEnum.Building);
        _playerBuildingSystem.DisableBuildingSystem();

        if (newSlot.Item == null)
        {
            Debug.Log($"Current Item : None");
            return;
        }

        Debug.Log($"Current Item : {_currentlySelectedSlot.Item.ItemType}");

        switch (_currentlySelectedSlot.Item.ItemType)
        {
            case ItemTypeEnum.BuildingItem:

                _playerState.AddPlayerState(PlayerStateEnum.Building);

                if (_currentlySelectedSlot.Item is BuildingItemDataSO)
                    _playerBuildingSystem.EnableBuildingSystem(_currentlySelectedSlot);
                else
                    Debug.LogWarning($"Item with ItemType BuildingItem isn't actualy a BuildingItem");
                break;

            case ItemTypeEnum.ToolItem:

                break;

            case ItemTypeEnum.WeaponItem:

                break;

            case ItemTypeEnum.EquipmentItem:

                break;

            case ItemTypeEnum.ActionItem:

                break;

            case ItemTypeEnum.ResourceItem:

                break;

            default:
                break;
        }
    }

    public void SubscribeToEvents()
    {
        EventBus.Instance.OnItemSelected += UpdatePlayerItem;
    }

    public void UnsubscribeFromEvents()
    {
        EventBus.Instance.OnItemSelected -= UpdatePlayerItem;
    }
}
