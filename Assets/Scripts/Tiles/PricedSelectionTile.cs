using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PricedSelectionTile : SelectionTile
{
    [field: SerializeField]
    public int cost;

    protected override bool VerifyOnMouseButtonDown()
    {
        if (!base.VerifyOnMouseButtonDown()) return false;
        if (false)
        {
            UIManager.Instance.DisplayErrorMessage("You don't have enough coins to buy this piece.");
            return false;
        }
        return true;
    }
}
