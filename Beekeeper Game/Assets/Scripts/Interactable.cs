using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
    ///
    /// Class encapsulating player-interactable object behaviour.
    ///
{
    // all methods in onInteracted have precondition playerInInteractableCondition()

    // unity event for calls to be done when a successful interaction is initiated
    public UnityEvent onInteracted;
    // unity event for calls to be done when the player starts hovering on the interactable object
    public UnityEvent onStartHover;
    // unity event for calls to be done when the player stops hovering on the interactable object
    public UnityEvent onEndHover;
    // the key used to interact with this object
    public KeyCode interactKey = KeyCode.E;
    // the object that pops up when hovering (for stuff like press indicators)
    public GameObject interactPopup = null;
    // bool representing hovering
    public bool hovering = false;

    // used to keep track of changes to the value of hovering
    private bool oldHovering = false;

    // returns true if the player can currently interact with this object
    public virtual bool playerInInteractableCondition()
    {
        return true;
    }

    private void OnValidate()
    {
        if (interactPopup != null)
            interactPopup.GetComponentInChildren<ButtonIndicatorUI>().setText(interactKey.ToString());
    }

    private void Start()
    {
        loadPopup();
    }

    protected virtual void loadPopup()
    {
        if (interactPopup != null)
        {
            ButtonIndicatorUI popupButtonUI = interactPopup.GetComponentInChildren<ButtonIndicatorUI>();
            popupButtonUI.setText(interactKey.ToString());
            onStartHover.AddListener(() =>
            {
                popupButtonUI.open();
            }
            );

            onEndHover.AddListener(() =>
            {
                popupButtonUI.close();
            }
            );
        }
    }

    protected void Update()
    {
        // deal with hovering
        if (!oldHovering && hovering)
        {
            onStartHover.Invoke();
        }
        else if (oldHovering && !hovering)
        {
            onEndHover.Invoke();
        }
        oldHovering = hovering;
    }
}
