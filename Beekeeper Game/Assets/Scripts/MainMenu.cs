using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string level;

    public GameObject[] canvas;

    public void Start()
    {
        
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(level);
    }

    public void CtrLoad()
    {
        canvas[0].SetActive(false);
        canvas[1].SetActive(true);
    }
    public void SaveLoad()
    {
        canvas[0].SetActive(false);
        canvas[2].SetActive(true);
    }

    public void BackBtn()
    {
        canvas[1].SetActive(false);
        canvas[0].SetActive(true);
    }

    public void BackMenuBtn()
    {
        canvas[2].SetActive(false);
        canvas[0].SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        print("Quit!");
    }
}
