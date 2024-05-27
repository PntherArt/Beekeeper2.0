using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectButton : MonoBehaviour
{
    public Beehive hive;
    public ProductObj product;
    public PlayerRaycast player;

    private void OnEnable()
    {
        Button collectButton = GetComponent<Button>();
        Fill fill = transform.parent.GetComponentInChildren<Fill>();
        collectButton.onClick.AddListener(() =>
            {
                if (player.inHandItem != null)
                {
                    // player has to be holding jar to interact with beehive
                    hive.gameObject.GetComponent<BeehiveInteraction>().fillJar(product);
                }
            });
    }

    private void OnDisable()
    {
        Button collectButton = GetComponent<Button>();
        collectButton.onClick.RemoveAllListeners();
    }



}
