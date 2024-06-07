using UnityEngine;

public class WFCCell : MonoBehaviour
{
    public bool isCollapsed;
    public WFCTile[] tileOptions;

    public void CreateCell(bool collapseState, WFCTile[] newTileOptions)
    {
        isCollapsed = collapseState;
        tileOptions = newTileOptions;
    }

    public void RecreateCell(WFCTile[] tiles)
    {
        tileOptions = tiles;
    }

}
