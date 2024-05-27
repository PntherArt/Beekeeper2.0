using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JarIndicatorVisability : MonoBehaviour
{
    public Beehive hive;

    void Update()
    {
        transform.Find("Jar").gameObject.active = hive.GetComponent<BeehiveInteraction>().hovering;
        transform.Find("FillSurragate").gameObject.active = hive.GetComponent<BeehiveInteraction>().hovering;
    }
}
