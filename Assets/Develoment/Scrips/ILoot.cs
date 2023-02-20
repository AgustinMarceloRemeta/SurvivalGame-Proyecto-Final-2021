using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ILoot : MonoBehaviour
{
    private int vida = 100;
    public GameObject[] Loot;
    public Transform Aparacion1, Aparacion2, Aparacion3, Aparacion4;

    void Update()
    {
        if (vida == 0) 
        {
            Destroy(gameObject);
        }
    }
     public void spawnLoot(int daño)
    {
        int i1 = Random.Range(0, Loot.Length);
        int i2 = Random.Range(0, Loot.Length);
        int i3 = Random.Range(0, Loot.Length);
        int i4 = Random.Range(0, Loot.Length);
        vida -= daño;
        Instantiate(Loot[i1], Aparacion1.position, Quaternion.identity);
        Instantiate(Loot[i2], Aparacion2.position, Quaternion.identity);
        Instantiate(Loot[i3], Aparacion3.position, Quaternion.identity);
        Instantiate(Loot[i4], Aparacion4.position, Quaternion.identity);
    }
}
