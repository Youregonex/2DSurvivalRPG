using UnityEngine;
using System.Collections.Generic;

public class PlayerState : MonoBehaviour, IEventListener
{
    [field: SerializeField] public List<PlayerStateEnum> CurrentPlayerStates { get; private set; }


    private void Awake()
    {
        CurrentPlayerStates = new List<PlayerStateEnum>();
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    public void AddPlayerState(PlayerStateEnum newState)
    {
        if(!CurrentPlayerStates.Contains(newState))
        {
            CurrentPlayerStates.Add(newState);
        }
    }

    public void RemovePlayerState(PlayerStateEnum stateToRemove)
    {
        if(CurrentPlayerStates.Contains(stateToRemove))
        {
            CurrentPlayerStates.Remove(stateToRemove);
        }
    }

    private void AddHoldingItemState() => CurrentPlayerStates.Add(PlayerStateEnum.HoldingItem);

    private void RemoveHoldingItemState() => CurrentPlayerStates.Remove(PlayerStateEnum.HoldingItem);

    public void SubscribeToEvents()
    {
        EventBus.Instance.OnPickItemInHand += AddHoldingItemState;
        EventBus.Instance.OnReleaseItemInHand += RemoveHoldingItemState;
    }

    public void UnsubscribeFromEvents()
    {
        EventBus.Instance.OnPickItemInHand -= AddHoldingItemState;
        EventBus.Instance.OnReleaseItemInHand -= RemoveHoldingItemState;
    }
}
