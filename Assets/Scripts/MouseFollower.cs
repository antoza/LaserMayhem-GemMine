using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class MouseFollower : MonoBehaviour
{
#if !DEDICATED_SERVER
    [field: SerializeField]
    private SpriteRenderer? m_FollowingTileRenderer;

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        worldPosition = new Vector3(worldPosition.x, worldPosition.y, -5);
        this.transform.position = worldPosition;
    }

    public void ChangeFollowingTile(Tile? tile)
    {
        if (tile != null && tile.Piece!)
        {
            m_FollowingTileRenderer!.sprite = tile!.Piece!.GetSprite();
        }
        else
        {
            m_FollowingTileRenderer!.sprite = null;
        }
    }
#endif
}
