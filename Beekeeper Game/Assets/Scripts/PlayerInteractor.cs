using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
    ///
    /// Class implementing interacting with interactable objects for the player.
    ///
{
    Rigidbody rb;
    public GameObject camPos;
    public LayerMask interactableLayers;
    public LayerMask groundLayers;
    public float interactRadius = 5;
    Interactable lastInteractable = null;
    RaycastHit hitter;

    private bool interactKeyDown = false;
    private bool mouse0Down = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    Interactable getInteractable()
    {
        // cast ray
        RaycastHit hit;
        if (Physics.Raycast(camPos.transform.position, camPos.transform.forward, out hit, interactRadius, interactableLayers, QueryTriggerInteraction.UseGlobal))
        {
            Debug.DrawRay(camPos.transform.position, camPos.transform.forward * interactRadius, Color.blue);
            // return null if no hit or hit a non interactable object
            if (!hit.collider.gameObject.GetComponent<Interactable>()) return null;
            
            return hit.collider.gameObject.GetComponent<Interactable>();
        } 
        return null;

            
    }

    private void Update()
    {
        Interactable interactable = getInteractable();
        if (interactable)
        {
            interactKeyDown = Input.GetKeyDown(interactable.interactKey);
        }
        mouse0Down = Input.GetKeyDown(KeyCode.Mouse0);
            
            
    }

    private void FixedUpdate()
    {

        Interactable interactable = getInteractable();
        if (interactable)
        {          
            if (interactable.playerInInteractableCondition())
            {
                // manage hovering actions
                if (interactable != lastInteractable && lastInteractable && lastInteractable.hovering)
                    lastInteractable.hovering = false;
                if (!interactable.hovering)
                    interactable.hovering = true;

                // interact if pressed interact key
                if (interactKeyDown) {
                    interactable.onInteracted.Invoke();
                    interactKeyDown = false;
                }

                // set last interactable
                lastInteractable = interactable;

                
            } else { // is a surface that isn't pick upable because there's currently an item in hand
                if (Physics.Raycast(Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.forward, out hitter, interactRadius, ~LayerMask.GetMask("Player")))
                {
                    Debug.DrawRay(Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.forward * interactRadius, Color.green);
                    if (mouse0Down) {
                    //if (Input.GetKeyDown(KeyCode.Mouse0)) {
                        GameObject player = GameObject.Find("Player");
                        player.GetComponent<PlayerRaycast>().placeObject(hitter);
                        mouse0Down = false;
                    }
                }
            }
            

        } else
        {
            if (lastInteractable && lastInteractable.hovering)
                lastInteractable.hovering = false;
            
            if (Physics.Raycast(Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.forward, out hitter, interactRadius, ~LayerMask.GetMask("Player")))
            {
                Debug.DrawRay(Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.forward * interactRadius, Color.green);
                if (mouse0Down) {
                //if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    GameObject player = GameObject.Find("Player");
                    player.GetComponent<PlayerRaycast>().placeObject(hitter);
                    mouse0Down = false;
                }
            }
        }
    }
}
