using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DayNightSwap : MonoBehaviour
{
    [Header("Colour Arrays")]
    public Color[] shadowCol;
    public Color[] sunCol;
    public Color fogCol;
    public Color[] grassCol;
    public Color[] bloomCol;

    [Header("Skybox Materials")]
    public Material[] skybox;

    [Header("Grass Shader")]
    public GameObject[] grassRend;

    [Header("Directional Sunlight")]
    public GameObject sunLight;

    [Header("Audio BGM")]
    public GameObject[] AudioBGM;

    [Header("VFX Animals")]
    public GameObject butterflies;
    public GameObject fireflies;

    [Header("Post Proc Bloom")]
    public Volume postProc;

    private void Start()
    {
        sunLight.gameObject.GetComponent<Light>();
        
        AudioBGM[0].GetComponent<GameObject>();
        AudioBGM[1].GetComponent<GameObject>();
        butterflies.GetComponent<GameObject>();
        fireflies.GetComponent<GameObject>();

        grassRend[0].GetComponent<GameObject>();
        grassRend[1].GetComponent<GameObject>();


    }


    void Update()
    {
        

    }


    public void DayLight()
    {
        //yeet the fog
        RenderSettings.fog = false;

        //skybox settings
        RenderSettings.skybox = skybox[0];

        //shadow settings
        RenderSettings.subtractiveShadowColor = shadowCol[0];

        //directional light sun
        sunLight.GetComponent<Light>().color = sunCol[0];

        grassRend[0].gameObject.GetComponent<Renderer>().material.GetColor("_TipColor");
        grassRend[1].gameObject.GetComponent<Renderer>().material.GetColor("_TipColor");

        grassRend[0].gameObject.GetComponent<Renderer>().material.SetColor("_TipColor", grassCol[0]);
        grassRend[1].gameObject.GetComponent<Renderer>().material.SetColor("_TipColor", grassCol[1]);

        AudioBGM[1].GetComponent<AudioSource>().Stop();
        AudioBGM[0].GetComponent<AudioSource>().Play();

        fireflies.SetActive(false);
        butterflies.SetActive(true);

        Bloom bloom;
        postProc.profile.TryGet(out bloom);
        bloom.tint.value = bloomCol[0];


    }

    public void NightLight()
    {

        //Fog settings
        RenderSettings.fog = true;
        RenderSettings.fogColor = fogCol;
        RenderSettings.fogDensity = 0.06f;

        //skybox settings
        RenderSettings.skybox = skybox[1];

        //shadow settings
        RenderSettings.subtractiveShadowColor = shadowCol[1];

        //directional light moon
        sunLight.GetComponent<Light>().color = sunCol[1];

        grassRend[0].gameObject.GetComponent<Renderer>().material.GetColor("_TipColor");
        grassRend[1].gameObject.GetComponent<Renderer>().material.GetColor("_TipColor");

        grassRend[0].gameObject.GetComponent<Renderer>().material.SetColor("_TipColor", grassCol[2]);
        grassRend[1].gameObject.GetComponent<Renderer>().material.SetColor("_TipColor", grassCol[3]); 

        AudioBGM[0].GetComponent<AudioSource>().Stop();
        AudioBGM[1].GetComponent<AudioSource>().Play();

        butterflies.SetActive(false);
        fireflies.SetActive(true);

        Bloom bloom;
        postProc.profile.TryGet(out bloom);
        bloom.tint.value = bloomCol[1];
    }

    
}
