using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepButtonIndicator : MonoBehaviour
{
    public GameObject Lighting;

    void Update()
    {
        transform.Find("ButtonIndicator").gameObject.active = DayCycle.timeOfDay >= Lighting.GetComponent<DayCycle>().nightStartHour || DayCycle.timeOfDay <= Lighting.GetComponent<DayCycle>().nightEndHour - 1;
    }
}
