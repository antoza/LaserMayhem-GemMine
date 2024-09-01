#if !DEDICATED_SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MenuButton
{
    public override void DoOnClick()
    {
        Application.Quit();
    }
}
#endif