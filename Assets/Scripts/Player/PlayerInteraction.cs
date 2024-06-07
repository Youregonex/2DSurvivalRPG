using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class PlayerInteraction : MonoBehaviour, IPlayer, IInitializeInteraction, IEventListener
{
    [SerializeField] private List<Transform> _interactablesList;

    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private LayerMask _obectMask;
    [SerializeField] private LayerMask _uiMask;

    [SerializeField] private IInteractable _currentInteraction;

    private float _raycastDistance = Mathf.Infinity;

    private void Awake()
    {
        SubscribeToEvents();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null)
            _interactablesList.Add(collision.GetComponent<Transform>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();

        if (interactable != null)
        {
            if (interactable == _currentInteraction)
            {
                interactable.StopInteracting(gameObject);
                _currentInteraction = null;
            }

            _interactablesList.Remove(collision.GetComponent<Transform>());
        }
    }

    private IInteractable FindClosestInteractible()
    {
        if (_interactablesList.Count == 0)
            return null;

        IInteractable interactable = _interactablesList.OrderBy(x => Vector3.Distance(transform.position, x.position))
                                                       .First().GetComponent<IInteractable>();

        return interactable;
    }

    private void InteractWithClosest()
    {
        if (_interactablesList.Count == 0)
            return;

        IInteractable interactable = FindClosestInteractible();

        StartInteraction(interactable);
    }

    private void StartInteraction(IInteractable interactable)
    {
        bool currentlyInterracting = _currentInteraction != null;

        if (currentlyInterracting && interactable != _currentInteraction)
        {
            StopAllInteractions();

            interactable.Interact(gameObject);
            _currentInteraction = interactable;
            return;
        }

        if (currentlyInterracting && interactable == _currentInteraction)
        {
            StopAllInteractions();
            return;
        }

        interactable.Interact(gameObject);
        _currentInteraction = interactable;
    }

    private void StopAllInteractions()
    {
        if (_currentInteraction != null)
            _currentInteraction.StopInteracting(gameObject);

        _currentInteraction = null;
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void CheckMouseClickForInteractions(Vector3 mousePosition)
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.transform.position, (mousePosition - Camera.main.transform.position), _raycastDistance, _obectMask);
        Debug.DrawRay(Camera.main.transform.position, (mousePosition - Camera.main.transform.position), Color.red, 10f);

        if (!hit || hit.transform.GetComponent<IInteractable>() == null)
            return;
        
        if (_interactablesList.Contains(hit.transform))
        {
            IInteractable interactable = hit.transform.GetComponent<IInteractable>();
            StartInteraction(interactable);
        }
    }

    public void SubscribeToEvents()
    {
        EventBus.Instance.OnInteractionKeyPressed += InteractWithClosest;
        EventBus.Instance.OnRMBClick += CheckMouseClickForInteractions;
    }

    public void UnsubscribeFromEvents()
    {
        EventBus.Instance.OnInteractionKeyPressed -= InteractWithClosest;
        EventBus.Instance.OnRMBClick -= CheckMouseClickForInteractions;
    }
}