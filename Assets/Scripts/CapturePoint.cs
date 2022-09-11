using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CapturePoint : MonoBehaviour
{
    public int progress = 0;
    bool captureNeeded = true;
    bool captureProgression = false;
    bool captured = false;
    public TMP_Text captureProgress;
    public Material enemyControl;
    public Material allyControl;
    bool m_Play;
    AudioSource m_Capture;
    bool m_ToggleChange;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material = enemyControl;
    }

    // Update is called once per frame
    void Update()
    {
        if(progress==100)
        {
            captureNeeded = false;
            m_Play = true;
            m_Capture = GetComponent<AudioSource>();
            m_Capture.Play();
            m_ToggleChange = false;
            //print("played audio");
        }

        if(captureNeeded)
        {
            captureProgress.text = progress.ToString()+"%";
        }
        else
        {
            GetComponent<MeshRenderer>().material = allyControl;
            captureProgress.text = "";
            if(!captured)
            {
                captured = true;
                FindObjectOfType<Player>().pointsCaptured++;
                FindObjectOfType<Player>().pointCaptured();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Player")
        {
           if(!captureProgression) StartCoroutine(captureCoolDown());
        }

    }

    IEnumerator captureCoolDown()
    {
        captureProgression = true;
        yield return new WaitForSeconds(0.3f);
        progress++;
        captureProgression = false;
    }


}
