using System;
using UnityEngine;
using System.Collections.Generic;

public class EventBus
{
    private static EventBus _instance;

    public static EventBus Instance
    {
        get
        {
            if (_instance == null)
                _instance = new EventBus();

            return _instance;
        }
    }

    // Player Events
    public Action OnInventoryKeyPressed;
    public Action OnLMBKeyPressed;
    public Action OnInteractionKeyPressed;
    public Action OnEscKeyPressed;
    public Action OnAttackAnimationStart;
    public Action OnAttackAnimationEnd;

    public void InventoryKeyPressed() => OnInventoryKeyPressed?.Invoke();
    public void LMBKeyPressed() => OnLMBKeyPressed?.Invoke();
    public void InteractionKeyPressed() => OnInteractionKeyPressed?.Invoke();
    public void EscKeyPressed() => OnEscKeyPressed?.Invoke();
    public void AttackAnimationStart() => OnAttackAnimationStart?.Invoke();
    public void AttackAnimationEnd() => OnAttackAnimationEnd?.Invoke();

    public Action<int> OnHotbarSlotSelected;
    public Action<Vector3> OnPlayerPositionRequested;
    public Action<Vector3> OnRMBClick;

    public void HotbarSlotSelected(int slotSelected) => OnHotbarSlotSelected?.Invoke(slotSelected);
    public void PlayerPositionRequested(Vector3 position) => OnPlayerPositionRequested?.Invoke(position);
    public void RMBClick(Vector3 mousePosition) => OnRMBClick?.Invoke(mousePosition);


    // Player Inventory Events
    public Action OnPlayerInventoryDisplayRequested;
    public Action OnInventoryChanged;
    public Action OnDynamicInventoryHideRequested;

    public void PlayerInventoryDisplayRequested() => OnPlayerInventoryDisplayRequested?.Invoke();
    public void InventoryChanged() => OnInventoryChanged?.Invoke();
    public void DynamicInventoryHideRequested() => OnDynamicInventoryHideRequested?.Invoke();

    public Action<InventorySlot> OnInventorySlotChanged;
    public Action<InventorySystem> OnDynamicInventoryDisplayRequested;
    public Action<InventorySlot> OnItemSelected;

    public void InventorySlotChanged(InventorySlot slot) => OnInventorySlotChanged?.Invoke(slot);
    public void DynamicInventoryDisplayRequested(InventorySystem inventory) => OnDynamicInventoryDisplayRequested?.Invoke(inventory);
    public void ItemSelected(InventorySlot slot) => OnItemSelected?.Invoke(slot);

    // Player CraftingSystem Events
    public Action OnCraftingDisplayRequested;

    public void CraftingDisplayRequested() => OnCraftingDisplayRequested?.Invoke();

    public Action<PlayerInventoryHolder, List<CraftingRecipeSO>> OnCraftMenuUpdateRequest;
    public Action<CraftingRecipeSO> OnItemCraftRequested;
    public Action<CraftingRecipeSO> OnCraftDetailsRequested;

    public void CraftMenuUpdateRequest(PlayerInventoryHolder inventory, List<CraftingRecipeSO> recipes) => OnCraftMenuUpdateRequest?.Invoke(inventory, recipes);
    public void ItemCraftRequested(CraftingRecipeSO recipe) => OnItemCraftRequested?.Invoke(recipe);
    public void CraftDetailsRequested(CraftingRecipeSO recipe) => OnCraftDetailsRequested?.Invoke(recipe);

    // UI Mouse Events
    public Action OnInventoryUIItemClick;
    public Action OnPointerExitInventoryUISlot;
    public Action OnPickItemInHand;
    public Action OnReleaseItemInHand;

    public void InventoryUIItemClick() => OnInventoryUIItemClick?.Invoke();
    public void PointerExitInventoryUISlot() => OnPointerExitInventoryUISlot?.Invoke();
    public void PickItemInHand() => OnPickItemInHand?.Invoke();
    public void ReleaseItemInHand() => OnReleaseItemInHand?.Invoke();

    public Action<CraftingRecipeSO> OnCraftOptionClick;
    public Action<InventorySlotUI> OnPointerEnterInventoryUISlot;

    public void CraftOptionClick(CraftingRecipeSO recipe) => OnCraftOptionClick?.Invoke(recipe);
    public void PointerEnterInventoryUISlot(InventorySlotUI slot) => OnPointerEnterInventoryUISlot?.Invoke(slot);

    // Test Events
    public Action OnTestAction;

    public void TestAction() => OnTestAction?.Invoke();

    private void LogEventTrigger(Type initializer, string eventName) => Debug.Log($"{initializer} Fired On{eventName} event");
}
