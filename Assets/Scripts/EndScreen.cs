using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class EndScreen : MonoBehaviour
{
    public TMP_Text kills;

    private void Update()
    {
        kills.text = "Thanks to the Australian Man's Efforts " + Player.emusKilled.ToString() + " Emus have been killed";
    }

}
