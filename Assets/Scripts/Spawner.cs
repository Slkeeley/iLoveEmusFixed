using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject emu;
    public float spawnDelay;
    public bool canSpawn = true;
    // Start is called before the first frame update
    void Start()
    {
        spawnEmu();
        StartCoroutine(spawnCooldown());
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn) spawnEmu();
    }

    void spawnEmu()
    {
        GameObject.Instantiate(emu);
        StartCoroutine(spawnCooldown());
    }

    IEnumerator spawnCooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnDelay);
        canSpawn = true;
    }
}
