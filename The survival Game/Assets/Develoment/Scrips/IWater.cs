using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IWater : MonoBehaviour
{
    public Material NewMaterial;
    public bool bebible;

    public void Cambio()
    {
        MeshRenderer render = GetComponent<MeshRenderer>();
        render.material= NewMaterial;
        bebible = true;
    }
}
