using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIstanceFrom : MonoBehaviour
{
    public GameObject FocusObject;
    //public GameObject Player;

    public float Distance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Distance = Vector3.Distance(FocusObject.transform.position, GameObject.FindWithTag("Player").transform.position);
        //Debug.Log(Distance);
        if (Distance <= 3) {
            if (Input.GetKey("e")) {
                Debug.Log("Pressed");
            }
        }
    }
}
