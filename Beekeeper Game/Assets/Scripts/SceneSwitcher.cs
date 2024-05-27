using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public GameObject[] GameCanv;
    public GameObject[] supOut;
    private bool invokeD = false;
    public string level;

    public AudioSource snd;

    void Start()
    {
        
    }

    private void Update()
    {
        if (GameCanv[2].activeInHierarchy && invokeD == false)
        {
            Invoke("btnUp", 2f);
            invokeD = true;
        }

        if (supOut[2].activeInHierarchy)
        {
            supOut[0].SetActive(false);
        }

    }

    public void LetsGo()
    {
        GameCanv[0].SetActive(false);
        GameCanv[1].SetActive(false);
        GameCanv[2].SetActive(true);
    }

    public void btnUp()
    {
        supOut[0].SetActive(true);
    }

    public void InsideSuup()
    {
        snd.Play();
        Invoke("next", 5f);
    }


    void next()
    {
        SceneManager.LoadScene(level);
    }
}
