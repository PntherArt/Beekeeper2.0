using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class ToggleMenu : MonoBehaviour
{
    // class for a UI menu that can be toggled on and off by a key.

    // key that toggles this menu
    
    public enum MenuMode { key, click };
    [Header("Menu mode")]
    public MenuMode mode = MenuMode.key;
    [Header("Key if menu mode is key:")]
    public KeyCode toggleKey;
    [Header("Other variables")]
    public ScreenManager manager;
    public UnityEvent onOpen;
    public UnityEvent onClose;

    // animator component of this menu
    protected Animator anim;

    // whether this menu frees the cursor and locks the camera when toggled open
    public bool freeCursor = true;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void open()
    {
        onOpen.Invoke();
        anim.SetBool("opened", true);
    }

    public void close()
    {
        onClose.Invoke();
        anim.SetBool("opened", false);
    }
    private void Update()
    {
        switch (mode)
        {
            case MenuMode.key:
                if (Input.GetKeyDown(toggleKey))
                    manager.onToggled(this);
                break;
        }
    }
}
