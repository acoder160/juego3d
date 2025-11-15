using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoScript : MonoBehaviour
{

    public float lookRadius = 10f;
    float atackRadius = 2f;

    Transform target;
    [SerializeField] GameObject player;
    [SerializeField] HealthBar health;
    public GameObject healthBar;

    NavMeshAgent agent;
    public Animator animator;
    public bool die = false;

    //Tiempo de espera
    private float lastActionTime;
    private float cooldownDuration = 5.0f;

    //Tiempo de espera
    private float lastActionTimeAtaque;
    private float cooldownAtaqueDuration = 0.9f;
    private bool ataque = false;
    private bool cooldownDie = false;

    private float lastActionTimeDeath;
    private float cooldownDeathDuration = 2f;


    void Start()
    {
        target = player.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        lastActionTime = Time.time;
        healthBar.SetActive(false);
      
    }

    // Update is called once per frame
    void Update()
    {
        // Al principio de Update()
        if (die)
        {
            cooldownDie = true;
            // BUG ARREGLADO: Tu lógica de cooldown estaba mal.
            if (cooldownDie)
            {
                lastActionTimeDeath = Time.time;
                cooldownDie = false; // <-- CAMBIO: Evita que el timer se reinicie

                // <-- AÑADIDO: Detiene al agente de forma segura
                if (agent.isOnNavMesh)
                {
                    agent.isStopped = true;
                    agent.ResetPath(); // Limpia su destino
                }
            }

            // Una vez que pase el tiempo, destruye el objeto
            if (Time.time > lastActionTimeDeath + cooldownDeathDuration)
            {
                Destroy(gameObject);
            }

            // <-- CAMBIO CRÍTICO: Sale del Update() si está muerto.
            //     Esto evita que llame a SetDestination() más abajo.
            return;
        }

        float distance = Vector3.Distance(target.position, transform.position);
      


        // --- ESTADO: ATACAR ---
        if (distance <= atackRadius && !die)
        {
            agent.SetDestination(transform.position);
            FaceTarget();

           
            animator.SetBool("IsAttacking", true);

           
            if (ataque == false)
            {
                ataque = true;
                lastActionTimeAtaque = Time.time;

            }


            if (Time.time > lastActionTimeAtaque + cooldownAtaqueDuration && ataque == true) { 
                health.takeDamage(); 
                lastActionTime = Time.time;
                ataque = false;
            }
           



            if (Time.time > lastActionTime + cooldownDuration)
            {
                
                lastActionTime = Time.time;
            }
        }

        // --- ESTADO: PERSEGUIR ---
        else if (distance <= lookRadius)
        {
           
            healthBar.SetActive(true);
            agent.SetDestination(target.position);

           
            animator.SetBool("IsAttacking", false);
           
        }

        // --- ESTADO: IDLE (INACTIVO) ---
        else
        {

            healthBar.SetActive(false);
            agent.SetDestination(transform.position);
            animator.SetBool("IsAttacking", false);
      
        }

     
        float animationSpeed = agent.velocity.magnitude;
   
        animator.SetFloat("Speed", animationSpeed);
    }

    // Rotate to face the target
    void FaceTarget()
    {
       
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

  


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}