using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject emu;
    public float spawnDelay;
    public bool canSpawn = false;
    // Start is called before the first frame update
    void Start()
    {
        spawnEmu();
        StartCoroutine(spawnCooldown());
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            canSpawn = false; 
            spawnEmu();
        }
    }

    void spawnEmu()
    {
        GameObject.Instantiate(emu);
        StartCoroutine(spawnCooldown());
    }

    IEnumerator spawnCooldown()
    {
        yield return new WaitForSeconds(spawnDelay);
        canSpawn = true;
    }
}
