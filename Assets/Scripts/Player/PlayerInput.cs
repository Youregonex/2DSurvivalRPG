using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MovementInput { get; private set; }

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        ManageMovementInput();
        ManageInventoryKey();
        ManageInteractionKey();
        ManageMouseClick();
        ManageEscKey();
        ManageHotbarSelection();
        TestAction();
    }

    private void ManageMovementInput()
    {
        MovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void TestAction()
    {
        if (Input.GetKeyDown(KeyCode.F))
            EventBus.Instance.TestAction();
    }

    private void ManageInventoryKey()
    {
        if(Input.GetKeyDown(KeyCode.Q))
            EventBus.Instance.InventoryKeyPressed();
        
    }

    private void ManageInteractionKey()
    {
        if(Input.GetKeyDown(KeyCode.E))
            EventBus.Instance.InteractionKeyPressed();
    }

    private void ManageEscKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            EventBus.Instance.EscKeyPressed();
    }

    private void ManageMouseClick()
    {
        Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;

        if (Input.GetMouseButtonDown(0))
            EventBus.Instance.LMBKeyPressed();


        if (Input.GetMouseButtonDown(1))
            EventBus.Instance.RMBClick(mouseWorldPosition);
    }

    private void ManageHotbarSelection()
    {
        int slotSelected;

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            slotSelected = 0;
            EventBus.Instance.HotbarSlotSelected(slotSelected);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            slotSelected = 1;
            EventBus.Instance.HotbarSlotSelected(slotSelected);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            slotSelected = 2;
            EventBus.Instance.HotbarSlotSelected(slotSelected);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            slotSelected = 3;
            EventBus.Instance.HotbarSlotSelected(slotSelected);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            slotSelected = 4;
            EventBus.Instance.HotbarSlotSelected(slotSelected);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            slotSelected = 5;
            EventBus.Instance.HotbarSlotSelected(slotSelected);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            slotSelected = 6;
            EventBus.Instance.HotbarSlotSelected(slotSelected);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            slotSelected = 7;
            EventBus.Instance.HotbarSlotSelected(slotSelected);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            slotSelected = 8;
            EventBus.Instance.HotbarSlotSelected(slotSelected);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            slotSelected = 9;
            EventBus.Instance.HotbarSlotSelected(slotSelected);
        }
    }

}
