using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowerWatering : MonoBehaviour
{
    //This controls the watering flowers system
    [Header("Watering Checkboxes")]
    public bool isWatered = false;
    public bool hasWaterCan = false;
    public bool isCanEmpty = false;
    private bool hasAlerted = false;

    [Header("Game Object References")]
    public GameObject pickUp;

    [Header("Watering Can Capacity + Press E")]
    public int waterLevel;
    private int defWaterLevel;

    [Header("Water Capacity UI")]
    public Slider uiWaterIndicator;
    public GameObject uiWaterSlider;

    [Header("Alerts")]
    public AlertManager alertManager;

    RaycastHit hit;


    private void Start()
    {
        pickUp.GetComponent<GameObject>();
        defWaterLevel = waterLevel;
        uiWaterSlider.GetComponent<GameObject>();
        uiWaterIndicator.GetComponent<Slider>().maxValue = waterLevel;
        uiWaterIndicator.GetComponent<Slider>().value = waterLevel;

    }


    private void Update()
    {
        //This holds the UI for the can fullness
        uiWaterIndicator.GetComponent<Slider>().value = waterLevel;

        if (waterLevel == 0)
        {
            isCanEmpty = true;
            
            if(isCanEmpty == true && hasAlerted == false)
            {
                alertManager.queueAlert("Water Can is empty!");
                hasAlerted = true;

            }
        }
        //this toggles whether or not the can is in the inventory
        if(pickUp.transform.childCount == 0)
        {
            hasWaterCan = false;
            uiWaterSlider.SetActive(false);
        }

        //this controls the tags that allow filling the can, the actual watering and the vfx triggered
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100.0f) && pickUp.transform.childCount != 0)
            {

                if (pickUp.transform.GetChild(0).CompareTag("Water Can") == true)
                {
                    uiWaterSlider.SetActive(true);    

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (hit.collider.CompareTag("River"))
                        {
                            print("River!");
                            FillTheCan();
                        }

                        if (hit.collider.CompareTag("Flower"))
                        {

                            print("Hit flower");
                            if (waterLevel != 0)
                            {
                                print(hit.transform.gameObject.name);

                                hasWaterCan = true;

                                if (isWatered == false)
                                {
                                    //pickUp.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                                    waterLevel--;
                                    StartCoroutine(FlowerSparkle());
                                }

                            print("Watering Active");
                        }
                    }

                } else if (pickUp.transform.GetChild(0).CompareTag("Water Can") == false)
                {
                    uiWaterSlider.SetActive(false);
                    hasWaterCan = false;
                }


            }

            

        }

    }

    //this controls the filling of the can in designated places
    public void FillTheCan()
    {
        if (pickUp.transform.GetChild(0).CompareTag("Water Can"))
        {
            hasWaterCan = true;

            if (Input.GetKey(KeyCode.E))
            {
                waterLevel = defWaterLevel;
                isCanEmpty = false;
                hasAlerted = false;
            }

        }

        if (pickUp.transform.GetChild(0).CompareTag("Water Can") == false)
        {
            hasWaterCan = false;
        }
    }

    //this controls the sparkle vfx after the flower has been watered successfully
    IEnumerator FlowerSparkle()
    {
        pickUp.transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(3f);
        hit.transform.GetChild(0).transform.gameObject.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(5f);
        isWatered = false;

    }

}
