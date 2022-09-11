using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

public class BaseEmu : MonoBehaviour
{

    [Header("AI Variables")]
    public NavMeshAgent agent; //allows emu to move
    public Transform player;//What enemy is the monster targetting
    public LayerMask whatIsGround;//What is legal for the enemy to walk on
    public LayerMask whatIsPlayer;
    public Vector3 walkPoint;//where will the enemy go to 
    bool walkPointSet;
    public float walkPointRange;
    public float sightRange, attackRange;
    public bool enemyInSight, enemyInAttackRange;//tell the monster to chase after a seen enemy and attack one if in range  
    bool dead = false; 

    [Header("Object Variables")]
    bool alreadyAttacked=false;
    public float attackDelay;
    public float health = 9;
    public GameObject head;
    public GameObject emuCanvas;
    public GameObject bloodEffect;
    public Image healthBar;
    public int emuDamage=5;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        bloodEffect.SetActive(false);
        head.SetActive(true);
        emuCanvas.SetActive(true);
    }

    private void Update()
    {
        if(health<=0)
        {
            dead = true; 
            StartCoroutine(emuDeath());
        }

        if (!dead)
        {
            enemyInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!enemyInSight && !enemyInAttackRange) patrol();
            if (enemyInSight && !enemyInAttackRange) aggro();
            if (enemyInSight && enemyInAttackRange) attack();
        }
        healthBar.fillAmount = Mathf.Clamp(health / 9, 0, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "gunProjectile")
        {
            health = health - 6;
        }
    }
    void patrol()
    {
        Debug.Log("patrolling");
        if (!walkPointSet) searchWalkPoint();

        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude <1.0f)
        {
            walkPointSet = false; 
        }
    }

    void searchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet=true; 
    }

    void aggro()
    {
        Debug.Log("Aggro drawn");
        agent.SetDestination(player.position);
    }

    void attack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            stab();
            alreadyAttacked = true;
            StartCoroutine(attackAgain());
        }
    }

   void stab()
    {
        Debug.Log("stab");
        head.GetComponent<EmuHead>().animator.Play("HeadStab");
        RaycastHit hitPlayer;
        if(Physics.Raycast(transform.position, transform.forward, out hitPlayer, attackRange+3f))
        {
            Player enemy = hitPlayer.transform.GetComponent<Player>();
            if (enemy != null)
            {
                enemy.takeDamage(emuDamage);
            }
        }
    }

    IEnumerator attackAgain()
    {
        yield return new WaitForSeconds(attackDelay);
        head.GetComponent<EmuHead>().animator.StopPlayback();//stop the emu head attack animation
        alreadyAttacked = false; 
    }

    public void takeDamage(int damage)
    {
        health = health - damage;
    }

    IEnumerator emuDeath()
    {
        agent.SetDestination(transform.position);
        head.SetActive(false);
        emuCanvas.SetActive(false);
        bloodEffect.SetActive(true);
        Vector3 newRotation = new Vector3(0, 90, -90f);
        transform.eulerAngles = newRotation;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        yield return new WaitForSeconds(3.0f);
        Player.emusKilled++; 
        Destroy(this.gameObject);
    }

}
