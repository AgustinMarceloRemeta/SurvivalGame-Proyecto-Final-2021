using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IEnemy : MonoBehaviour
{
    int vida = 100;
    public Animator animator;
    public NavMeshAgent agent;
     GameObject jugador;
     bool ataca;
    public float distancia;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("ataque", 1, 1);
    }

    void Update()
    {
        if (Vector3.Distance(jugador.transform.position, gameObject.transform.position) < distancia && Vector3.Distance(jugador.transform.position, gameObject.transform.position) > 4f)
        {
            agent.SetDestination(jugador.transform.position);
            Corre();

        }
        else if (Vector3.Distance(jugador.transform.position, gameObject.transform.position) < 4f)
        {
            NoCorre();
            agent.SetDestination(gameObject.transform.position);
            animator.SetBool("Ataca", true);
            transform.LookAt(jugador.transform.position);
            ataca = true;
        }
        else
        {
            agent.SetDestination(gameObject.transform.position);
            NoCorre();
            ataca = false;
        }
    }
    void Corre()
    {
        animator.SetBool("Corre", true);
        animator.SetBool("Ataca", false);
        ataca = false;

    }
    void NoCorre()
    {
        animator.SetBool("Corre", false);
    }
    void ataque()
    {
        if (ataca)
        {
            IStatus status = FindObjectOfType<IStatus>();
            status.daño(5);
        }
    }
     public void BajaVida(int cantidad)
    {
        vida -= cantidad;
        if (vida <= 0) Destroy(gameObject);
    }
}
