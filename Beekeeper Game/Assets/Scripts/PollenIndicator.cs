using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollenIndicator : MonoBehaviour
{
    // TODO: OBSOLETE- CHANGE WITH BEEHIVE UI
    public Shader meterShader;
    public Texture jarBack;
    public Texture jarFront;
    Material mat;
    Beehive beeHive;

    void Awake()
    {
        beeHive = GetComponentInParent<Beehive>();
        mat = new Material(meterShader);
        mat.SetTexture("_BackTex", jarBack);
        mat.SetTexture("_FrontTex", jarFront);
        mat.SetColor("_Color", new Color(0.921f, 0.662f, 0.216f));
        GetComponent<MeshRenderer>().material = mat;
    }


    void Update()
    {
        // set level of pollen indicator depending on pollen in beehive
        // mat.SetFloat("_Level", beeHive.Pollen);
    }
}
