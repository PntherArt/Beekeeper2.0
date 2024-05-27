using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class BeehiveCanvas : MonoBehaviour
{
    public void updateBeeHive(Beehive hive) {
        foreach (Transform obj in transform.GetComponentsInChildren<Transform>())
        {
            if(obj.name == "Fill") {
                obj.GetComponent<Fill>().hive = hive;
            }

            if(obj.name == "Collect") {
                obj.GetComponent<CollectButton>().hive = hive;
            }
        }
    }
}
