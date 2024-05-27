using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jiggle : MonoBehaviour
{
    Renderer rend;
    Vector3 lastPos;
    Vector3 velocity;
    Vector3 lastRot;  
    Vector3 angularVel;
    public float maxJiggle = 1f;
    public float jiggleSpeed;
    public float settleTime;
    float jiggleAmtX;
    float jiggleAmtZ;
    float jiggleAmtToAddX;
    float jiggleAmtToAddZ;
    float jiggleCurve;
    float utime = 0.5f;
    
    void Start()
    {
        rend = GetComponent<Renderer>();

        jiggleAmtX.ToString("0.00");
        jiggleAmtToAddX.ToString("0.00");

        jiggleAmtZ.ToString("0.00");
        jiggleAmtToAddZ.ToString("0.00");
    }
    private void Update()
    {
        //decreasing jiggle over time
        utime += Time.deltaTime;
        jiggleAmtToAddX = Mathf.Lerp(jiggleAmtToAddX, 0, Time.deltaTime * (settleTime));
        jiggleAmtToAddZ = Mathf.Lerp(jiggleAmtToAddZ, 0, Time.deltaTime * (settleTime));

        // sine for the jiggle decrease
        jiggleCurve = 2 * Mathf.PI * jiggleSpeed;
        jiggleAmtX = jiggleAmtToAddX * Mathf.Sin(jiggleCurve * utime);
        jiggleAmtZ = jiggleAmtToAddZ * Mathf.Sin(jiggleCurve * utime);

       //bc I am lazy when making shaders!
        rend.material.SetFloat("_JiggleX", jiggleAmtX);
        rend.material.SetFloat("_JiggleZ", jiggleAmtZ);

        // velocity antics, we know her well
        velocity = (lastPos - transform.position) * Time.deltaTime;
        angularVel = transform.rotation.eulerAngles - lastRot;

        
        jiggleAmtToAddX = Mathf.Clamp((velocity.x * (angularVel.z * 0.2f)) + maxJiggle, -maxJiggle, maxJiggle);
        jiggleAmtToAddZ = Mathf.Clamp((velocity.z * (angularVel.x * 0.2f)) + maxJiggle, -maxJiggle, maxJiggle);

        lastPos = transform.position;
        lastRot = transform.rotation.eulerAngles;

        //A lot of the velocity script above has been edited from one of my lecturers help scripts from last year! He always gives
        //permission to reuse his scripts so S.O  to him! Thanks Max! Saved me a lot of debugging :)
    }



}