using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IBuilding : MonoBehaviour
{
    public List<string> RequisitosCumplidos;
    public List<IRequisito> Requisitos;
    public Material NuevoMat;
    public TextMesh texto;
    public GameObject barco, TextoObj;
    public bool EsBarco;
    
    void Update()
    {
        foreach (var item in Requisitos )
        {

            foreach (var item2 in RequisitosCumplidos)
            {
                if(item.name == item2)
                {
                    Remove(item2);
                    RequisitosCumplidos.Remove(item2);  
                    break;
                }
            }
            if (item.cantidad == 0) { 
                Requisitos.Remove(item);
                break;
            }
        }
        if (Requisitos.Count == 0)
        {
            texto.text = "";
            cambio();
        }
        else
        {
            string text = "Faltan:";
            foreach (var item in Requisitos)
            {
                text +="\n" + " " + item.name + "(" + item.cantidad+")";
            }
            texto.text = text;
        }
    }

    void cambio()
    {
        MeshRenderer render = GetComponent<MeshRenderer>();
        render.material = NuevoMat;
        Collider Box = GetComponent<Collider>();
        Box.isTrigger = false;
        if (EsBarco)
        {
            barco.transform.GetChild(1).gameObject.SetActive(true);
            barco.transform.GetChild(2).gameObject.SetActive(true);
            barco.transform.GetChild(3).gameObject.SetActive(true);
            barco.transform.GetChild(4).gameObject.SetActive(true);
            gameObject.tag = "EndGame";
        }
        else gameObject.tag = "agarrable";
    }

    void Remove(string i)
    {
        foreach (var item in Requisitos)
        {
            if (i == item.name) item.cantidad--;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) TextoObj.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) TextoObj.SetActive(false);
    }
}
[System.Serializable]
public class IRequisito
{
    public string name;
    public int cantidad;

    public IRequisito(string name, int cantidad)
    {

        this.name = name;
        this.cantidad = cantidad;
    }
}
