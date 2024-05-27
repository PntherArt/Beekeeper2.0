using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseMenu : MonoBehaviour
{
    public GameObject PauCanv;
    public GameObject SettingsCanv;
    public GameObject Canv;
    public CameraMovement camController;
    public ScreenManager sManager;
    bool isTrig = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isTrig == false)
        {
            isTrig = true;
            sManager.ChangeScreen(null);
            Cursor.lockState = CursorLockMode.None;
            camController.locked = true;
            Cursor.visible = true;
            DayCycle.timePaused = true;
            Canv.SetActive(false);
            PauCanv.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isTrig == true)
        {
            isTrig = false;
            Cursor.lockState = CursorLockMode.Locked;
            camController.locked = false;
            Cursor.visible = false;
            DayCycle.timePaused = false;
            PauCanv.SetActive(false);
            SettingsCanv.SetActive(false);
            Canv.SetActive(true);
        }

    }

    public void Resume()
    {
        isTrig = false;
        Cursor.lockState = CursorLockMode.Locked;
        camController.locked = false;
        Cursor.visible = false;
        DayCycle.timePaused = false;
        PauCanv.SetActive(false);
        Canv.SetActive(true);
    }

    public void Settings()
    {
        isTrig = true;
        sManager.ChangeScreen(null);
        Cursor.lockState = CursorLockMode.None;
        camController.locked = true;
        Cursor.visible = true;
        DayCycle.timePaused = true;
        Canv.SetActive(false);
        PauCanv.SetActive(false);
        SettingsCanv.SetActive(true);
    }

    public void Back()
    {
        isTrig = true;
        sManager.ChangeScreen(null);
        Cursor.lockState = CursorLockMode.None;
        camController.locked = true;
        Cursor.visible = true;
        DayCycle.timePaused = true;
        Canv.SetActive(false);
        SettingsCanv.SetActive(false);
        PauCanv.SetActive(true);
    }

    public void Quit()
    {
        print("Quit!");
        Application.Quit();
    }
}
