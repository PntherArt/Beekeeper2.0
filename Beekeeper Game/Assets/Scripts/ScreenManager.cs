using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public CameraMovement camMovement;

    // screen that is currently open
    [SerializeField] ToggleMenu openMenu = null;
    public ToggleMenu OpenMenu
    {
        get { return openMenu; }
    }

    private void Awake()
    {
        openMenu = null;
    }


    // close the currently open screen (if it exists) and open the new one
    public void ChangeScreen(ToggleMenu nextMenu)
    {
        if (nextMenu != null && nextMenu.freeCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            camMovement.locked = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            camMovement.locked = false;
        }
        if (openMenu != null)
            openMenu.close();
        if (nextMenu != null)
            nextMenu.open();
        openMenu = nextMenu;
    }

    // toggle the given screen.
    // i.e. if it's open, close it, and if its not open, change to it
    public void onToggled(ToggleMenu menu)
    {
        if (menu == openMenu)
        {
            openMenu.close();
            Cursor.lockState = CursorLockMode.Locked;
            camMovement.locked = false;
            openMenu = null;
        }
        else
        {
            ChangeScreen(menu);
        }
    }

}
