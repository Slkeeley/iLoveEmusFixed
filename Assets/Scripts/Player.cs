using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    private BoxCollider playerHitbox;

    //Floats related to the player's stats
    [Header("Player Characteristics")]
    public float healthPool=100f;
    public float normalSpeed;
    public float sprintSpeed;
    public float frontBack;
    public float leftRight;
    public bool isSprinting = false;
    public int pointsCaptured;
    public static int emusKilled;
    [Header("UI Elements")]
    public TMP_Text bulletText;
   public TMP_Text captureProgressText;
    public Image healthBar;
    public GameObject captureText;
    public GameObject hitmarker;
    //Components needed to do stuff
    [Header("Components")]
    //Player's rigidbody
    public GameObject gun;
    public GameObject gunProjectile; 
    // Start is called before the first frame update
    void Start()
    {
        playerHitbox = GetComponent<BoxCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        updateUI();
        captureText.SetActive(false);
        hitmarker.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(pointsCaptured>=3)
        {
            finish();
        }
            
        if (healthPool <= 0)
        {
            die();
        }

        //Check if Left Shift is being held down
        if (Input.GetKey(KeyCode.LeftShift) == true)
        {
            //Sets the isSprinting state to true if they are holding Shift
            isSprinting = true;
            //Gets the W/S inputs and makes it into a float based on the sprint speed you choose in the inspector
            frontBack = Input.GetAxis("Vertical") * sprintSpeed;
            //Gets the A/D inputs and makes it into a float based on the sprint speed you choose in the inspector
            leftRight = Input.GetAxis("Horizontal") * sprintSpeed;
        }
        else
        {
            //Sets the isSprinting state to false if they are not holding Shift
            isSprinting = false;
            //Gets the W/S inputs and makes it into a float based on the normal speed you choose in the inspector
            frontBack = Input.GetAxis("Vertical") * normalSpeed;
            //Gets the A/D inputs and makes it into a float based on the speed you choose in the inspector
            leftRight = Input.GetAxis("Horizontal") * normalSpeed;
        }

        //Converts the movement floats to be based on time instead of frames
        frontBack *= Time.deltaTime;
        leftRight *= Time.deltaTime;

        //Translates the player based on the WASD input in the X and Z axis
        transform.Translate(leftRight, 0, frontBack);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            if(GetComponentInChildren<Gun>().bullets<1)
            {
                gun.SetActive(false);
                GameObject.Instantiate(gunProjectile, new Vector3(transform.position.x+0.5f, transform.position.y+0.5f, transform.position.z+2f), Quaternion.identity);
            }
        }
        updateUI();

    }

    //Respawn reloads the level from scratch
    public void die()
    {
        SceneManager.LoadScene("DeathScene");
    }
    public void finish()
    {
        SceneManager.LoadScene("VictoryScene");
    }
    private void updateUI()
    {
        bulletText.text = gun.GetComponent<Gun>().bullets.ToString();
        captureProgressText.text = pointsCaptured.ToString() + "/3";
        healthBar.fillAmount = Mathf.Clamp(healthPool /100, 0, 1f);
    }

    public void takeDamage(int emuDamage)
    {
        healthPool = healthPool - emuDamage;
    }

    public  void pointCaptured()
    {
        captureText.SetActive(true);
        StartCoroutine(removeCaptureText());
    }

    IEnumerator removeCaptureText()
    {
        yield return new WaitForSeconds(2.0f);
        captureText.SetActive(false);
    }
}
