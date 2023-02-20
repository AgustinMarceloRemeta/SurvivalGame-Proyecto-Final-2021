using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISpawn : MonoBehaviour
{
     public GameObject[] Objetos;
    public List<INumber> numeros;
    public int CantObjetos, xMax, xMin, zMax, zMin;
    public float Y;

    void Start()
    {
        while (numeros.Count < CantObjetos)
        {
            int randomX = Random.Range(xMin, xMax);
            int randomz = Random.Range(zMin, zMax);
            var NewNumber = new INumber(randomX, randomz);
            bool entra = true;
            foreach (var item in numeros)
            {
                if (NewNumber == item) entra = false;
            }
            if (entra) numeros.Add(NewNumber);
        }
        foreach (var item in numeros)
        {
            int i= Random.Range(0, Objetos.Length);
            Instantiate(Objetos[i],new Vector3(item.X, Y, item.Z),Quaternion.identity);
        }
       
    }

}
[System.Serializable]
public class INumber{
    public int X;
    public int Z;

    public INumber(int X, int Z)
    {
        this.X = X;
        this.Z = Z;
    }
}