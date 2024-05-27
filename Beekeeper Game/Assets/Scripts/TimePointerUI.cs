using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimePointerUI : MonoBehaviour
{
    public int wiggliness = 20;
    public RectTransform pointerRect;
    public TMP_Text timeDisplay;
    public RectTransform dayBarRect;
    public DayCycle dayCycle;
    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void updateUI(float time)
    {
        // precondition: 0 <= time < 24
        float parentRectWidth = dayBarRect.sizeDelta.x;
        float thisWidth = pointerRect.sizeDelta.x;
        float dx = (517) / 24;

        //Debug.Log(((time - dayCycle.nightEndHour) % 24));

        // set position of pointer depending on time
        // (x%m + m)%m - better modulo taken from https://stackoverflow.com/questions/1082917/mod-of-negative-number-is-melting-my-brain
        pointerRect.anchoredPosition = new Vector2(
            -517 * 0.5f + ((((time - dayCycle.nightEndHour) % 24) + 24) % 24) * dx,
            dayBarRect.sizeDelta.y * Mathf.Sin(Time.realtimeSinceStartup * 2f) * 0.05f - dayBarRect.sizeDelta.y * 0.25f
        );

        // set hour display depending on time
        timeDisplay.text = "Day " + GlobalVariables.day + " | " + DayCycle.getHourString(time);


    }

    // Update is called once per frame
    void Update()
    {

    }
}
