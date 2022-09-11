using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    Vector3 playerPos;
    Transform player; 
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("egg instantiated");
        player = GameObject.Find("Player").transform;
        playerPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        StartCoroutine(despawnEgg());
    }

    // Update is called once per frame
    void Update()
    {
       transform.position = Vector3.MoveTowards(this.transform.position, playerPos, 10* Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            other.GetComponent<Player>().healthPool = other.GetComponent<Player>().healthPool - 10;
            Destroy(this.gameObject);
        }
        if(other.tag!=null)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator despawnEgg()
    {
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }
}
