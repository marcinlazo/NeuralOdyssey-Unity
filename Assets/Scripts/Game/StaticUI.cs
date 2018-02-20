using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticUI {

	public static bool ShowingCursor { get; private set; }

    public static void ShowCursor(bool show)
    {
        if (show)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        ShowingCursor = show;
    }
}
