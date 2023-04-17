using System.Collections.Generic;
using UnityEngine;
using Beautify.Universal;
using UnityEngine.Rendering;


public class HackModeManager : MonoBehaviour
{
    public GameObject HackablesParent; // all hackable items should be children of this
    public Material hologramMat;

    private List<Renderer> renderers = new List<Renderer>();


    public Color nightVisionColor;
    public Color curNightVisionColor;
    public Color goalNightVisionColor;
    public float DistanceThreshValue;
    public float curDistanceThreshValue;
    public float goalDistanceThreshValue;
    public float curAbberationVal;
    public float lowerDistanceThreshValue;
    private bool isHackMode;
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private List<Material[]> originalMaterials = new List<Material[]>();

    private GameObject player;

    void Start()
    {
        GetAllSpriteRenderersInChildren(HackablesParent.transform);

        player = GameObject.FindWithTag("Player");
        GetSpriteRenderersInChildrenRecursive(player.transform);

    }

    void GetAllSpriteRenderersInChildren(Transform parent)
    {
        spriteRenderers.Clear(); // Clear the list of sprite renderers
        originalMaterials.Clear(); // Clear the list of original materials

        // Get all SpriteRenderer components in children recursively
        GetSpriteRenderersInChildrenRecursive(parent);

        // Display the number of sprite renderers found
        Debug.Log("Number of sprite renderers found: " + spriteRenderers.Count);
    }

    void GetSpriteRenderersInChildrenRecursive(Transform parent)
    {
        // Loop through all child transforms of the parent
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();

            // If the child has a SpriteRenderer component, add it to the list
            if (spriteRenderer != null)
            {
                spriteRenderers.Add(spriteRenderer);

                // Store the original materials of the sprite renderer
                originalMaterials.Add(spriteRenderer.materials);
            }

            // Recursively search for SpriteRenderer components in the child's children
            GetSpriteRenderersInChildrenRecursive(child);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("SWAP HHACK MODE");
            isHackMode = !isHackMode;

            if (isHackMode)
            {
                HackablesParent.layer = LayerMask.NameToLayer("NoPostProcess");
                ChangeLayerOfChildren(HackablesParent.transform, "NoPostProcess");

                goalNightVisionColor = nightVisionColor;
                Beautify.Universal.BeautifySettings.settings.nightVision.value = true;

                Beautify.Universal.BeautifySettings.settings.outlineDistanceFade.value = 100f;
                Beautify.Universal.BeautifySettings.settings.outlineDistanceFade.value = 100f;
                curDistanceThreshValue = 10.0f;
                curAbberationVal = 10.0f;
                SwapHackableMaterialsTo(hologramMat);
                CameraShake.ShakeOnce(.04f, 8f);
            }
            else
            {
                HackablesParent.layer = LayerMask.NameToLayer("Default");
                ChangeLayerOfChildren(HackablesParent.transform, "Default");

                goalNightVisionColor = Color.black;
                goalNightVisionColor.a = 0;
                Beautify.Universal.BeautifySettings.settings.nightVision.value = false;
                SwapHackableMaterialsBack();
            }
        }

        curNightVisionColor = Color.Lerp(curNightVisionColor, goalNightVisionColor, 0.6f);
        Beautify.Universal.BeautifySettings.settings.nightVisionColor.value = curNightVisionColor;

        goalDistanceThreshValue = lowerDistanceThreshValue;
        curDistanceThreshValue = Mathf.Lerp(curDistanceThreshValue, goalDistanceThreshValue, .025f);
        Beautify.Universal.BeautifySettings.settings.outlineDistanceFade.value = curDistanceThreshValue;

        curAbberationVal = Mathf.Lerp(curAbberationVal, 0.0f, .8f);
        Beautify.Universal.BeautifySettings.settings.chromaticAberrationIntensity.value = curAbberationVal;
    }


    void ChangeLayerOfChildren(Transform parent, string layerName)
    {
        // Change the layer of the parent GameObject
        parent.gameObject.layer = LayerMask.NameToLayer(layerName);

        // Iterate through all the children of the parent
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            // Recursively call the function on each child
            ChangeLayerOfChildren(child, layerName);
        }
    }


    // Swap materials to a new set of materials
    public void SwapHackableMaterialsTo(Material newMaterial)
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            // Create a new array of materials with the same length as the original materials
            Material[] newMaterials = new Material[spriteRenderer.materials.Length];

            // Set all elements of the new materials array to the new material
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = newMaterial;
            }

            // Assign the new materials array to the sprite renderer
            spriteRenderer.materials = newMaterials;
        }
    }

    // Swap materials back to the original materials
    public void SwapHackableMaterialsBack()
    {
        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            SpriteRenderer spriteRenderer = spriteRenderers[i];
            Material[] originalMats = originalMaterials[i];

            // Assign the original materials back to the sprite renderer
            spriteRenderer.materials = originalMats;
        }
    }

}
