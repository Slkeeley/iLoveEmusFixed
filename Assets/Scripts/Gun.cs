using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class Gun : MonoBehaviour
{
    [Header("Gun Variables")]
    public int damage = 3; 
    public float range = 100f;
    public int bullets = 6;
    public bool canShoot=true; 
    public Camera fpsCam;
    public Animator animator;
    public GameObject muzzleFlash;

    // Start is called before the first frame update
    void Start()
    {
        bullets = 6;//default number of bullets players start with
        muzzleFlash.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0)&&bullets>0)//if a player presses left click with bullets still need to be in the chamber
        {
            if (canShoot)//Make sure there is a delay between shots
            {
                Debug.Log("weapon fired");
                shoot();
                canShoot = false; 
                StartCoroutine(attackDelay());
            }
        }

    }

    void shoot()
    {
        //sound
        animator.Play("GunRecoil");
        bullets--;
       muzzleFlash.SetActive(true);
        RaycastHit hit;
      if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))//check if the raycast from the camera hit an enemy
        {
            Debug.Log(hit.transform.name);
            BaseEmu emu = hit.transform.GetComponentInParent<BaseEmu>();
         
//            BaseEmu emu = hit.transform.GetComponentInParent<BaseEmu>();
            if(emu !=null)
            {
                GetComponentInParent<Player>().hitmarker.SetActive(true);
                StartCoroutine(hitmarker());
                emu.takeDamage(damage); 
            }
            if(hit.transform.name=="JeepMu")
            {
                Debug.Log("Jeep Has been hit");
                GetComponentInParent<Player>().hitmarker.SetActive(true);
                StartCoroutine(hitmarker());
                hit.transform.GetComponent<JeepMu>().takeDamage(damage);
            }
        }
    }

    IEnumerator attackDelay()
    {
        yield return new WaitForSeconds(0.3f);
        muzzleFlash.SetActive(false);
        yield return new WaitForSeconds(0.7f);
        animator.StopPlayback();
        canShoot = true;
    }
    IEnumerator hitmarker()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponentInParent<Player>().hitmarker.SetActive(false);
    }
}

