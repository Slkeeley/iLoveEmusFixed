using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProjectile : MonoBehaviour
{
    [Header("Projectile Variables")]
    public bool returningToPlayer = false;
    public bool chaining = false;
    public float speed;
    public float timeThrown;
    public int maxChains = 3; 
    public int emusHit; 
    [Header("Communication w/ Other Scripts")]
    Transform player;
    Rigidbody rb;
    Transform cameraPos;
    Transform nextEmu;
    bool m_Play;
    AudioSource m_Throw;
    bool m_ToggleChange;

    private void Start()
    {
        Debug.Log("projectile instantiated");
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").transform;//find both the player and camera objects in the scene
        cameraPos = GameObject.Find("fpsCam").transform;
        StartCoroutine(beginReturn());
        m_Play = true;
        m_Throw = GetComponent<AudioSource>();
        m_Throw.Play();
        m_ToggleChange = false;
    }

    private void Update()
    {
        if (emusHit >= 3)
        {
            chaining = false;
            returningToPlayer = true;
        }
        if (!returningToPlayer)
        {
            rb.velocity = cameraPos.forward * speed;//projectile is thrown out in the direction the player is looking
        }

        if(returningToPlayer)//once the gun is returning to the player reset the velocity and have it transform to the player
        {
          rb.velocity = new Vector3(0,0,0);
          transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
          transform.LookAt(player.transform);
        }

        if(chaining)
        {
            rb.velocity = new Vector3(0, 0, 0);
            transform.position = Vector3.MoveTowards(this.transform.position, nextEmu.transform.position, speed * Time.deltaTime);
            transform.LookAt(nextEmu.transform);
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if(!chaining) returningToPlayer = true;//hitting anything will cause the gun to boomerang back to the player even before the timer ends.
        if(other.tag=="Player")//When the gun returns to the player get rid of the projectile and reset the hitscan weapon
        {
            other.GetComponent<Player>().gun.SetActive(true);
            other.GetComponentInChildren<Gun>().bullets = 6;
            other.GetComponentInChildren<Gun>().canShoot = true;
            Destroy(this.gameObject);
        }

        if(other.tag=="Emu")
        {
            hitEmu();
        }
    }

    void hitEmu()//if the gun projectile hits an emu see if there are emus for it to chain too.
    {
        emusHit++;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.tag=="Emu")
            {
                Debug.Log("Found another emu");
                nextEmu = hitCollider.transform;
                chaining = true;
                returningToPlayer = false;
                break;
            }
            else
            {
                chaining = false;
                returningToPlayer = true; 
            }
        }
    }

    IEnumerator beginReturn()
    {
        yield return new WaitForSeconds(timeThrown);
        if(!returningToPlayer) returningToPlayer = true;
    }
}
