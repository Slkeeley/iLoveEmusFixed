using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class JeepMu : MonoBehaviour
{

    [Header("AI Variables")]
    public NavMeshAgent agent; //allows emu to move
    public Transform player;//What enemy is the monster targetting
    public LayerMask whatIsGround;//What is legal for the enemy to walk on
    public LayerMask whatIsPlayer;
    public Vector3 walkPoint;//where will the enemy go to 
    bool walkPointSet;
    public float walkPointRange;
    public float sightRange;
    public bool enemyInSight;
    public bool charging = false;
    public bool waiting=true;
    public bool posReset=true;
    public Vector3 playerPos;

    [Header("Object Variables")]
    public float health = 12;
    public Image healthBar;
    public int emuDamage = 5;
    public GameObject spawnEmus;
    public float speed;
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        charging = false;
        waiting = false; 
    }

    private void Update()
    {
        if (health <= 0)
        {
            Die();
        }

        if (!waiting)
        {
            if (!charging&&posReset)
            {
                Debug.Log("Is Not charging");
                enemyInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
                if (!enemyInSight) patrol();
                if (enemyInSight)
                {
                    playerPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
                    charging = true;
                    posReset = false; 
                }
            }
            if (charging)
            {
                charge();
            }
        }
        healthBar.fillAmount = Mathf.Clamp(health / 15, 0, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "gunProjectile")
        {
            Debug.Log("Collided with gun");
            health = health - 6;
        }
        if(other.tag=="Player")
        {
            other.GetComponent<Player>().healthPool= other.GetComponent<Player>().healthPool - 15;
            StartCoroutine(crash());
        }
    }
    void patrol()
    {
        if (!walkPointSet) searchWalkPoint();

        if (walkPointSet) agent.SetDestination(walkPoint);

        transform.LookAt(walkPoint);
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

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }

    void charge()
    {
        Debug.Log("attempting to chagre");
        transform.LookAt(playerPos);
        transform.position = Vector3.MoveTowards(this.transform.position, playerPos, speed * Time.deltaTime);

        Vector3 distanceToPlayerPos = transform.position - playerPos;
        if(distanceToPlayerPos.magnitude<0.1f)
        {
            StartCoroutine(crash());
        }
     }

    
    IEnumerator crash()
    {
        waiting = true;
        charging = false;
        yield return new WaitForSeconds(6.0f);
        waiting = false;
        posReset = true; 
    }

    public void takeDamage(int damage)
    {
        health = health - damage;
    }

    void Die()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject.Instantiate(spawnEmus, transform.position, Quaternion.identity);
        }
        //car crash sound
        Destroy(this.gameObject);
    }


}
