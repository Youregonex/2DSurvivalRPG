
public class ContainerBuilding : Building
{
    protected InventoryHolder _containerInventory;

    protected override void Awake()
    {
        base.Awake();

        _containerInventory = GetComponent<InventoryHolder>();
    }
}
