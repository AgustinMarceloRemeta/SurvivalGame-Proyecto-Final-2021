using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IChest : MonoBehaviour
{
    public List<GameObject> Objetos;
    public Animator animator;

    public void abierto()
    {
        animator.SetBool("Abierto", true);
    }
    public void cerrado()
    {
        animator.SetBool("Abierto", false);
    }
}
