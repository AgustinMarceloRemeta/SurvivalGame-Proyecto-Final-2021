using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IMenu : MonoBehaviour
{
public void Jugar()
    {
        SceneManager.LoadScene("Main"); 
    }
    public void salir()
    {
        Application.Quit(); 
    }
}
