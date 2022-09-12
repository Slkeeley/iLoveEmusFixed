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
        if (canSpawn&&SpawnMax.currEnemies<150)
        {
            canSpawn = false; 
            spawnEmu();
        }
    }

    void spawnEmu()
    {
        GameObject.Instantiate(emu, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
        SpawnMax.currEnemies++;
        StartCoroutine(spawnCooldown());
    }

    IEnumerator spawnCooldown()
    {
        yield return new WaitForSeconds(spawnDelay);
        canSpawn = true;
    }
}
