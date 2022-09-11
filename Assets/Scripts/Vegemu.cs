using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegemu : MonoBehaviour
{
    public int healthGiven;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            if (other.GetComponent<Player>().healthPool >= 100) { return; }//player cannot use health pickup at full health; 
            else
            {
                //play sound?
                other.GetComponent<Player>().healthPool = other.GetComponent<Player>().healthPool + healthGiven;
                 if(other.GetComponent<Player>().healthPool>100)
                  {
                      other.GetComponent<Player>().healthPool = 100;
                  }
                Destroy(this.gameObject);
            }
        }
    }
}
