using System;
using System.Collections.Generic;
using UnityEngine;

public class CarView : MonoBehaviour
{
    [SerializeField] private MeshRenderer carBodyMesh = null;
    [SerializeField] private List<GameObject> wheels = null;
    [SerializeField] private List<MeshRenderer> rimMeshes = null;

    public void SetCarColorAndRims()
    {
        // Material[] carBodyMaterials = GetCarBodyMaterials();
        // foreach (Material material in carBodyMaterials)
        // {
        //     material.color = GameManager.GetCarColor();
        // }
        //
        // if (GameManager.GetRimMaterial() != null)
        // {
        //     SetRimMaterial(GameManager.GetRimMaterial());
        // }
    }

    public void SetCarColor(Color color)
    {
        Material[] carBodyMaterials = GetCarBodyMaterials();
        foreach (Material material in carBodyMaterials)
        {
            material.color = color;
        }
    }
    
    public void SetRimMaterial(Material material)
    {
        foreach (MeshRenderer meshRenderer in rimMeshes)
        {
            Material[] materials = meshRenderer.materials;
            materials[0] = material;
            meshRenderer.materials = materials;
        }
    }

    public Material GetRimMaterial()
    {
        Material[] materials = rimMeshes[0].materials;
        return materials[0];
    }

    public Material[] GetCarBodyMaterials()
    {
        MeshRenderer meshRenderer = carBodyMesh.GetComponent<MeshRenderer>();
        return meshRenderer.materials;
    }
}
