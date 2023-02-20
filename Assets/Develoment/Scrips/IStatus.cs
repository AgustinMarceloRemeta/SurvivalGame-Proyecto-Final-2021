using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IStatus : MonoBehaviour
{
    public float health, hunger, thirst;
    public float healthTick, hungerTick, thirstTick;
    public float healthMax, hungerMax, thirstMax;
    public float thirstMin;
    public Image healthIm, hungerIm, thirstIm;
    IInventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<IInventory>();
    }
    private void Update()
    {
        if (health <= 0)
        { 
            inventory.muerte();
            health = 100;
            thirst = 0;
            hunger = 0;
            inventory.UpdateUI();
        }
        Control();

        SeteoUI();

        Maximo();
    }

    private void Maximo()
    {
        if (health > healthMax) health = healthMax;
        if (thirst < thirstMin) thirst = thirstMin;

    }

    private void SeteoUI()
    {
        healthIm.fillAmount = health / healthMax;
        hungerIm.fillAmount = hunger / hungerMax;
        thirstIm.fillAmount = thirst / thirstMax;  
    }

    private void Control()
    {
        if (health < healthMax&& thirst<20 && hunger < 20) health += Time.deltaTime / healthTick;
        if (hunger < hungerMax) hunger += Time.deltaTime / hungerTick;
        if (thirst < thirstMax) thirst += Time.deltaTime / thirstTick;
        if (thirst == 100) health-= Time.deltaTime/healthTick;
        if (hunger == 100) health -= Time.deltaTime / healthTick;
    }
    public void Comida(int cantidad)
    {
        hunger -= cantidad;
    }
    public void bebida(int cantidad)
    {
        thirst -= cantidad;
    }
    public void daño(int cantidad)
    {
        health -= cantidad;
    }
}
