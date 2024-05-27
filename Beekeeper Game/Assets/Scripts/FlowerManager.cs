using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlowerManager : MonoBehaviour
{
    public static List<GameObject> flowers = new List<GameObject>();
    public GameObject pickUpSlot;

    // integer tracking num of flowers
    public static int numFlowers { get { return flowers.Count; } private set { } }


    public static ref List<GameObject> getFlowerList() { return ref flowers; }


    public static GameObject getRandomFlowerObject(System.Predicate<GameObject> match)
    {
        if (numFlowers != 0)
        {

            List<GameObject> foundFlowers = flowers.FindAll(match);
            int numFoundFlowers = foundFlowers.Count;
            if (numFoundFlowers != 0)
            {
                int rand = Random.Range(0, foundFlowers.Count);
                return foundFlowers[rand];
            }
            else
            {
                Debug.Log("No flowers found with the given criterion!");
            }
        }
        else
        {
            Debug.LogError("Tried to get random flower when there are none!");
        }
        return null;
    }

    public void fillFlowers()
    {
        foreach (GameObject flowerObject in flowers)
        {
            Flower flower = flowerObject.GetComponent<Flower>();
            flower.regenProduct();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // get all flowers that are in the scene, either in the Flowers object or in the hand of the player
        flowers = GameObject.FindGameObjectsWithTag("Flower").ToList<GameObject>().FindAll(
            flowerObject =>
            {
                return (flowerObject.transform.parent == transform || flowerObject.transform.parent == pickUpSlot.transform)
                    && flowerObject.activeSelf;
            }
        );
    }
}
