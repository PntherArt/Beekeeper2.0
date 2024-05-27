using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Highlight : MonoBehaviour
{
    [SerializeField]
    public Material highlightMaterial;

    private Renderer[] renderers;

    private List<Material> originalMaterials;

    public void Start() {
        renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = getMaterials();
    }

    public void highlightObject() {
        foreach (Renderer renderer in renderers) {
            renderer.material = highlightMaterial;
        }
    }

    public void unhighlightObject() {
        for (int i = 0; i < renderers.Length; i++) {
            renderers[i].material = originalMaterials[i];
        }
    }

    private List<Material> getMaterials()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        List<Material> materials = new List<Material>();
        for (int i = 0; i < renderers.Length; i++)
        {
            materials.AddRange(renderers[i].materials);
        }

        return materials;

    }

    /*public void ToggleHighlight(bool val)
    {
        //List<Material> materials = getMaterials();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        List<Material> originalMaterials = getMaterials();
        if (val)
        {
            foreach (Renderer renderer in renderers) {
                renderer.material = highlightMaterial;
            }
            /*Renderer renderer = GetComponent<Renderer>();
            if (renderer != null) {
                renderer.material = highlightMaterial;
            }*/
            // foreach (var material in materials)
            // {
            //     material.EnableKeyword("_EMISSION");
            //     material.SetColor("_EmissionColor", color);
            // }
        /*
        else
        {
            for (int i = 0; i < renderers.Length; i++) {
                renderers[i].material = originalMaterials[i];
            }
            // foreach (var material in materials)
            // {
            //     material.DisableKeyword("_EMISSION");
            // }
        }
    }*/
}
