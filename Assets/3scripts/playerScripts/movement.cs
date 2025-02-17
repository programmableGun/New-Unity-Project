﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{

    public Rigidbody rb; //= GameObject.Find("bean").gameObject.GetComponent<Rigidbody>();
    public float forceAmt = 300f;
    public float jumpMultiplyer = 1f;
    private float velocity = 0f;
    public bool canJump = true;

    //public GameObject winScreen;
    public AudioManager audioManager;
    public CameraScript cameraScript;
    public bloom bloom;

    public MenuScript menuScript;
    public scoreManager scrManager;
    public ScriptManger scriptManger;
    public PowerUpHandler powerUpHandler;
    
    public bool controlsEnabled = true;
    public bool nextLevelLocked = true;
    
    void Start()
    {
        // assign stuff
        
        audioManager = GameObject.FindGameObjectWithTag("mainScript").GetComponent<AudioManager>();
        bloom = GameObject.Find("bloom").GetComponent<bloom>();
        menuScript = GameObject.FindGameObjectWithTag("canvas").GetComponent<MenuScript>();
        scrManager = GameObject.FindGameObjectWithTag("mainScript").GetComponent<scoreManager>();
        scriptManger = GameObject.FindGameObjectWithTag("mainScript").GetComponent<ScriptManger>();
        powerUpHandler = GameObject.FindGameObjectWithTag("mainScript").GetComponent<PowerUpHandler>();
        resetPosition(); // puts bean over the spawn pad
        
    }
    //this is starting to become speghetti code and i want to fix it
    public void onKeypressMove() {  // basic movement with a rigidbody
        if (controlsEnabled)
        {
            if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space)) && canJump)
            {

                setVelocity(forceAmt * jumpMultiplyer, 2);
                audioManager.playerJump.Play();
                canJump = false;
                bloom.DoBloom(false);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {

                rb.AddForce(new Vector3(0f, -forceAmt, 0f));


            }
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(new Vector3(0f, 0f, forceAmt));
                

            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(new Vector3(0f, 0f, -forceAmt));
                
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(new Vector3(-forceAmt, 0f, 0f));
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(new Vector3(forceAmt, 0f, 0f));
            }
            if (Input.GetKeyDown(KeyCode.E)) {
                //run power up
                powerUpHandler.usePowerup();
            }

        }
        else {
            if (Input.GetKey(KeyCode.R))
            { //press r to respawn
                controlsEnabled = true;
                resetPosition();
                canJump = true;
                bloom.DoBloom(true);
                GetComponent<player>().deadScreen.gameObject.SetActive(false);
                GetComponent<player>().particle.Stop();
                scrManager.resetCombo();
                audioManager.playerDied.Stop();
            }
            
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="val"> the value on how much you want to force this object </param>
    /// <param name="index">1 = x, 2 = y, 3 = z</param>
    public void setVelocity(float val,int index) {
        this.velocity = val;
        if (index == 1) { rb.velocity = new Vector3(velocity, rb.velocity.y, rb.velocity.z); }
        if (index == 2) { rb.velocity = new Vector3(rb.velocity.x, velocity, rb.velocity.z); }
        if (index == 3) { rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, velocity); }

    }
    public int getVelocity() {
        return (int)rb.velocity.z; 
    } 
    public void resetPosition() {
        rb.gameObject.transform.position = new Vector3(0f, 10f, 0f);
        setVelocity(0, 3);
        setVelocity(0, 2);
        setVelocity(0, 1);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("ground")) {
            canJump = true;
            bloom.DoBloom(true);
            scrManager.resetCombo();
        }
        if (collision.gameObject.tag.Equals("endingBlock")) {
            nextLevelLocked = false;
            GameObject.FindGameObjectWithTag("mainScript").GetComponent<maxDistanceHandler>().addMaxDistance(this.gameObject.transform.position.z);
            GameObject.FindGameObjectWithTag("mainScript").GetComponent<PowerUpHandler>().clearAll();
            //winScreen.SetActive(true);
            canJump = true;
            bloom.DoBloom(true);
            scriptManger.nextLevel();
            resetPosition();

        }            
            


    }
    public void CheckHasFallinToDeath()
    {
        if (gameObject.transform.position.y < -15) {
            resetPosition();
            //aaaaaaaaaaaaaaaaaaaaaaahhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh

        }
    }



}
