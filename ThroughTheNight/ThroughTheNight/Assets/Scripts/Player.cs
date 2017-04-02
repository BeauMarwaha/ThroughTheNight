using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Author: Kat Weis
/// Represents the Player Object
/// </summary>
public class Player : Entity
{
    //fields
    public float knockback;
    public GameObject bulletPrefab;
    //fields for shooting cooldown
    private float timer;
    public float coolDown; //time between shots //.5
    //fields for invincibility frames
    private bool invincible;//whether or not the player is currently invincible
    public float invinTime;//length of time that you are invincible after being hit
    private float timerInvul;
    //player state enum for animations and correct orientation
    private PlayerState pState;
    private Animator animate;
    private enum PlayerState
    {
        MovingLeft,
        MovingRight,
        FacingLeft,
        FacingRight
    }

    // Use this for initialization
    protected override void Start ()
    {
        animate = this.GetComponent<Animator>();
        //instantiate timer to be higher than cooldown so that you can fire immediately
        timer = coolDown + 1;
        //set player attributes
        speed = 11f;
        attack = 5;
        //setup for invincibility frames
        timerInvul = 0;
        invincible = false;

        pState = PlayerState.FacingRight;//default start state is facing the right
    }
	
	// Update is called once per frame
	protected override void Update ()
    {
        if (GameManager.GM.currentState != State.Message && GameManager.GM.currentState != State.Over && GameManager.GM.currentState != State.Secret)
        {
            //if the player is in the facing left state swap its texture
            if (pState == PlayerState.FacingLeft || pState == PlayerState.MovingLeft)
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            else
                gameObject.GetComponent<SpriteRenderer>().flipX = false;

            direction = Vector3.zero;//reset direction to zero
            velocity = Vector3.zero;//reset velocity to zero

            Move();

            //check if currently invincible
            if (invincible == true)
            {
                //if the player has been invincible for longer than they should be turn it off
                if (timerInvul > invinTime)
                {
                    invincible = false;
                    timerInvul = 0;
                }
                else//otherwise increment the timer
                {
                    timerInvul += Time.deltaTime;
                }
            }


            //check for if the player's health is gone
            if (GameManager.GM.healthNum <= 0)
            {
                Death();
            }

            //if the player clicks, fire a projectile at where the mouse is currently
            if (Input.GetMouseButtonDown(0) && timer > coolDown)
            {
                Attack();
                timer = 0;
            }
            //increment timer
            timer += Time.deltaTime;
        }
    }

    /// <summary>
    /// method to move the player
    /// </summary>
    protected override void Move()
    {
        if (Input.GetKey(KeyCode.D))
        {
            direction.x += 1;
            pState = PlayerState.MovingRight;
            animate.SetBool("Moving", true);
        }else if (Input.GetKey(KeyCode.A))
        {
            direction.x -= 1;
            pState = PlayerState.MovingLeft;
            animate.SetBool("Moving", true);
        }else if (Input.GetKeyUp(KeyCode.D))
        {
            pState = PlayerState.FacingRight;
            animate.SetBool("Moving", false);
        }else if (Input.GetKeyUp(KeyCode.A))
        {
            pState = PlayerState.FacingLeft;
            animate.SetBool("Moving", false);
        }

        Vector3 location = transform.position;

        //calculate velocity from direction and speed times delta time so it is framerate independent
        velocity += direction.normalized * speed * Time.deltaTime;

        //update location by adding velocity
        location = transform.position + velocity;

        //clamp the x value so that the player cannot leave the room/screen
        location.x = Mathf.Clamp(location.x, -9f, 9f);

        //upload new position
        transform.position = new Vector3(location.x, transform.position.y, transform.position.z);

        //move the camera with the player
        MoveCamera();
    }

    /// <summary>
    /// Method to handle when the player dies
    /// </summary>
    protected override void Death()
    {
        GameManager.GM.ChangeHealth(0);//set health to zero if it was negative
    }

    /// <summary>
    /// method to handle when the player is attacked and no facing direction is specified
    /// </summary>
    public override void TakeDamage(int damageTaken)
    {
        //don't apply if in invincibility frames
        if (invincible == true)
        {
            return;
        }
        //decrease health
        GameManager.GM.ChangeHealth(-damageTaken);

        //apply knockback
        Knockback(true);

        //apply invincibility
        invincible = true;

    }

    /// <summary>
    /// method to handle when the player is attacked
    /// </summary>
    public void TakeDamage(int damageTaken, bool faceRight)
    {
        //don't apply if in invincibility frames
        if (invincible == true)
        {
            return;
        }
        //decrease health
        GameManager.GM.ChangeHealth(-damageTaken);

        //apply knockback
        Knockback(faceRight);

        //apply invincibility
        invincible = true;
    }

    /// <summary>
    /// method to handle when the player attacks
    /// </summary>
    protected override void Attack()
    {
        //shoot a projectile
        GameObject orb = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0,0, transform.rotation.z)) as GameObject;

        //play shot sound
        GameManager.GM.aSource.PlayOneShot(GameManager.GM.audioClips[7]);

        //set orbs direction
        //get the position of the mouse in local/screen position and convert it to world position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos -= gameObject.transform.position;
        //use arctan to determine angle betweeen clock hand and mouse
        float mouseRot = Mathf.Atan2(mousePos.y, mousePos.x);
        //convert angle from rads to deg and add 180 to compensate for being backwards
        mouseRot = Mathf.Rad2Deg * mouseRot + 180f;
        //set the rotation angle as an Euler angle
        orb.transform.rotation = Quaternion.Euler(0,0, mouseRot)  ; 

    }

    /// <summary>
    /// applies a pushback on the player depending on which direction they were attacked from
    /// </summary>
    /// <param name="faceRight">direction</param>
    private void Knockback(bool faceRight)
    {
        if(faceRight)//knockback left
        {
            transform.position = new Vector3(transform.position.x - knockback, transform.position.y, transform.position.z);
            return;
        }
        //else knockback right
        transform.position = new Vector3(transform.position.x + knockback, transform.position.y, transform.position.z);
    }

    //moves the camera with the play but within the room
    private void MoveCamera()
    {
        //maybe offset the location off from the player // player to the left

        Vector3 location = transform.position;

        //get camera
        Camera cam = Camera.main;

        //clamp x location to in room
        location.x = Mathf.Clamp(location.x, -3.35f, 3.5f);

        //set the cameras transform
        Camera.main.transform.position = new Vector3(location.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
    }

    /// <summary>
    /// restore some health to the player
    /// </summary>
    public void Restore()
    {
        //increase health
        GameManager.GM.ChangeHealth(2);
    }
}
