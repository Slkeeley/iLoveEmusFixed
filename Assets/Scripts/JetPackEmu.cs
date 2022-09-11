using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.AI; 

public class JetPackEmu : MonoBehaviour
{

    [Header("AI Variables")]
    //allows emu to move
    public NavMeshAgent agent; //allows emu to move
    public Transform player;//What enemy is the monster targetting
    public LayerMask whatIsAir;//What is legal for the enemy to walk on
    public LayerMask whatIsPlayer;
    public Vector3 walkPoint;//where will the enemy go to 
    bool walkPointSet;
    public float walkPointRange;
    public float sightRange, attackRange;
    public bool enemyInSight, enemyInAttackRange;//tell the monster to chase after a seen enemy and attack one if in range  
    [Header("Object Variables")]
    bool alreadyAttacked = false;
    public float attackDelay;
    public float health = 6;
    public Image healthBar;
    public GameObject Egg;
    public float speed = 10f; 


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        if (health <= 0)
        {
            Die(); 
        }
        Debug.Log(walkPointSet);
            enemyInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!enemyInSight && !enemyInAttackRange) patrol();
            if (enemyInSight && !enemyInAttackRange) aggro();
            if (enemyInSight && enemyInAttackRange) attack();
    
        healthBar.fillAmount = Mathf.Clamp(health / 6, 0, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "gunProjectile")
        {
            Debug.Log("Collided With Gun");
            health = health - 6;
        }
    }
    void patrol()
    {
        Debug.Log("patrolling");
        if (!walkPointSet) searchWalkPoint();

        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1.0f)
        {
            walkPointSet = false;
        }
    }

    void searchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        walkPointSet = true;
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsAir)) walkPointSet = true;
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

        if (!alreadyAttacked)
        {
            shootEgg();
            alreadyAttacked = true;
            StartCoroutine(attackAgain());
        }
    }

    void shootEgg()
    {
        Debug.Log("attackign");
        //instantiate projectile
    }

    IEnumerator attackAgain()
    {
        yield return new WaitForSeconds(attackDelay);
      
        alreadyAttacked = false;
    }

    public void takeDamage(int damage)
    {
        health = health - damage;
    }

   
    void Die()
    {
        Player.emusKilled++;
        Destroy(this.gameObject);
    }

}
