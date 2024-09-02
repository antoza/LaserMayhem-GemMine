using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WideMessageButton : MenuButton
{
    public override void DoOnClick()
    {
        Destroy(transform.parent.gameObject);
    }
}