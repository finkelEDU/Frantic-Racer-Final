using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 50;
    [SerializeField] private float handling = 100;
    [SerializeField] private float acceleratorMultiplier = 0;

    RaceControlX raceControlx;

    private bool btn_up, btn_down, btn_left, btn_right, btn_restart;

    private float maxAccel = 1;

    //private int powerup = 0;
    public int powerupTimer = 0;

    public RaceControlX raceControlX;

    private AudioSource playerAudio;
    public AudioClip finishSound;
    public AudioClip engineIdle;
    public AudioClip engineMoving;

    public bool raceActive = true;

    public int playerCheckpoint = 1;
    public int playerLap = 1;

    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        playerAudio.loop = true;
        playerAudio.Play();

        GameObject raceControlScript = GameObject.Find("EventSystem");
        raceControlX = raceControlScript.GetComponent<RaceControlX>();
    }

    void Update(){
        if(btn_restart)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(raceActive)
        {
            btn_left = Input.GetKey(KeyCode.LeftArrow);
            btn_right = Input.GetKey(KeyCode.RightArrow);
            btn_restart = Input.GetKeyDown(KeyCode.R);

            transform.Translate(-(Vector3.forward * maxSpeed * Time.deltaTime * acceleratorMultiplier));

            if(btn_left)
            {
                transform.Rotate(Vector3.up, -(handling * Time.deltaTime));
            }

            if(btn_right)
            {
                transform.Rotate(Vector3.up, (handling * Time.deltaTime));
            }

            if(acceleratorMultiplier <= 0)
            {
                if(playerAudio.clip != engineIdle)
                {
                    playerAudio.clip = engineIdle;
                    playerAudio.PlayDelayed(0);
                }
            }

            if(acceleratorMultiplier > 0)
            {
                if(playerAudio.clip != engineMoving)
                {
                    playerAudio.Stop();
                    playerAudio.clip = engineMoving;
                    playerAudio.PlayDelayed(0);
                }
            }
        }

        if(powerupTimer > 0){
            powerupTimer--;

            if(powerupTimer <= 0){
                maxAccel = 1;
            }
        }

        acceleratorMultiplier -= 0.0006f;

        if(acceleratorMultiplier < 0)
        {
            acceleratorMultiplier = 0;
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Cone"))
        {
            Rigidbody rb = collision.rigidbody;
            rb.AddForce(Vector3.up * 0.55f, ForceMode.Impulse);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        btn_up = Input.GetKey(KeyCode.UpArrow);
        btn_down = Input.GetKey(KeyCode.DownArrow);

        if(collision.gameObject.CompareTag("Wall"))
        {
            acceleratorMultiplier = Mathf.Max(-(acceleratorMultiplier * 0.2f), -(acceleratorMultiplier * 0.8f));
        }

        if(raceActive)
        {
            if(btn_up){
                if(collision.gameObject.CompareTag("Road"))
                {
                    if(acceleratorMultiplier < 1 * maxAccel)
                    {
                        acceleratorMultiplier += 0.04f;
                    }

                    if(acceleratorMultiplier > 1 * maxAccel)
                    {
                        acceleratorMultiplier = 1 * maxAccel;
                    }
                }

                if(collision.gameObject.CompareTag("Grass"))
                {
                    if(acceleratorMultiplier < 0.65 * maxAccel)
                    {
                        acceleratorMultiplier += 0.04f;
                    }

                    if(acceleratorMultiplier > 0.65f * maxAccel)
                    {
                        acceleratorMultiplier = 0.65f * maxAccel;
                    }
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Checkpoint"))
        {
            CheckPointX check = other.gameObject.GetComponent<CheckPointX>();

            if(check.checkpointNumber == playerCheckpoint)
            {
                playerCheckpoint++;
            }

            if(playerCheckpoint > 5)
            {
                switch(playerLap)
                {
                    case 1: playerCheckpoint = 1; playerLap++; break;
                    case 2: RaceEnd(); break;
                }
            }
        }

        if(other.gameObject.CompareTag("Powerup1"))
        {
            //Give the player a turbo boost of 1.5x the max speed
            powerupTimer = 800;
            maxAccel = 1.5f;
            acceleratorMultiplier = 1;

            Destroy(other.gameObject);
        }
    }

    void RaceEnd()
    {
        if(raceControlX.timer < Global.bestTime)
        {
            Global.bestTime = raceControlX.timer;
        }

        playerAudio.loop = false;
        playerAudio.Stop();
        playerAudio.PlayOneShot(finishSound, 1.0f);
        SceneManager.LoadScene("Finish Screen");
    }
}