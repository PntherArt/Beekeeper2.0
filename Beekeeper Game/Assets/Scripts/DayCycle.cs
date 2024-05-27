using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DayCycle : MonoBehaviour
{
    public GlobalVariables gVar;
    public float realMinutesPerGameHour = 1;
    [Range(0, 23)] public int nightStartHour = 20;
    [Range(0, 23)] public int nightEndHour = 5;
    public static bool timePaused = false;
    public UnityEvent[] hourlyEvents = new UnityEvent[24];
    public TimePointerUI timePointerUI;

    public GameObject sunLight;
    public GameObject clouds;

    //TODO: make this a static variable
    [Range(0, 23)] public static int timeOfDay = 0;

    private static float inGameRealTime = 0;
    private DayNightSwap dnSwap;

    private void Awake()
    {
        dnSwap = GetComponent<DayNightSwap>();
    }

    private void Start()
    {
        // set day/night switch events
        hourlyEvents[nightStartHour].AddListener(() => dnSwap.NightLight());
        hourlyEvents[nightEndHour].AddListener(() => dnSwap.DayLight());

        sunLight.gameObject.GetComponent<Transform>();
        clouds.gameObject.GetComponent<Transform>();

        skipToNextDay();
        
    }

    public float gameTimeToRealTime(float time)
    {
        return time * realMinutesPerGameHour * 60;
    }

    public void playerSleep() {
        skipToNextDay();
        dnSwap.DayLight();
    }

    public void skipToNextDay()
    {
        if (timeOfDay >= nightEndHour)
        {
            GlobalVariables.day++;
        }

        inGameRealTime = gameTimeToRealTime(nightEndHour);
    }

    public static string getHourString(float fractionalTime)
    {
        // returns a string given a time from 0 to 24 in the form hh:m0

        int hr = Mathf.FloorToInt(fractionalTime);
        int adjHr = hr % 24;
        string hr_str = (adjHr < 10) ? "0" + adjHr : adjHr.ToString();

        int min = Mathf.FloorToInt((fractionalTime - hr) * 60);
        // string min_str = (min < 10) ? "0" + min : min.ToString();
        string min_str = (min < 10) ? "00" : (min - (min % 10)).ToString();

        return hr_str + ":" + min_str;
    }

    float getTimeFactor()
    {
        return 1 / (realMinutesPerGameHour * 60);
    }

    public bool isNight()
    {
        float fractionalTime = inGameRealTime * getTimeFactor();
        if (nightStartHour > nightEndHour)
            return nightStartHour <= fractionalTime || nightEndHour > fractionalTime;
        return nightStartHour <= fractionalTime && nightEndHour > fractionalTime;


    }

    public void SunRotate()
    {
        sunLight.transform.localRotation = Quaternion.Euler((((inGameRealTime * getTimeFactor())) / 26 * 360f) - 90, 90, 0);

    }

    public void CloudRotate()
    {

        clouds.transform.eulerAngles = new Vector3(transform.eulerAngles.y, (inGameRealTime * getTimeFactor()) /24 * 360f);

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("in game time: " + inGameRealTime + ", deltaTime: " + Time.deltaTime + " timePaused: " + timePaused);
        if (!timePaused)
        {
            // increment in game time
            inGameRealTime += Time.deltaTime;

            float fractionalTime = inGameRealTime * getTimeFactor();
            // calculate current hour
            int currHour = Mathf.FloorToInt(fractionalTime);

            SunRotate();
            CloudRotate();

            // set hour if changed
            if (currHour > timeOfDay)
            {
                // if over 23, next day!
                if (currHour >= 24)
                {
                    // subtract one day in real seconds from in game timer
                    //inGameTime -= realMinutesPerGameHour * 60 * 24;
                    // not doing the above and resetting to 0 because otherwise float imprecision will accumulate
                    inGameRealTime = 0;
                    // set game hour to midnight
                    timeOfDay = 0;
                    // switch to next day
                    GlobalVariables.day++;
                }
                else // otherwise just update 
                {
                    timeOfDay = currHour;
                }

                // invoke the events for this hour
                hourlyEvents[timeOfDay].Invoke();
            }

            // update UI
            timePointerUI.updateUI(fractionalTime);
        }
    }
}
