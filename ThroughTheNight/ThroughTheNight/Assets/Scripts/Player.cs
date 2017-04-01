using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    //fields
    public float knockback;
    public GameObject bulletPrefab;
    //fields for shooting cooldown
    private float timer;
    public float coolDown; //time between shots //.5
    private bool invincible;//whether or not the player is currently invincible
    public float invinTime;//length of time that you are invincible after being hit
    private float timerInvul;
    private PlayerState pState;

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
        //instantiate timer to be higher than cooldown so that you can fire immediately
        timer = coolDown + 1;
        speed = 5;
        attack = 5;
        Spawn(Vector3.zero, Vector3.zero);
        timerInvul = 0;
        invincible = false;

        pState = PlayerState.FacingRight;//default start state is facing the right
    }
	
	// Update is called once per frame
	protected override void Update ()
    {
        //if the player is in the facing left state swap its texture
        if(pState == PlayerState.FacingLeft)
        gameObject.GetComponent<SpriteRenderer>().flipX = true;
        else
            gameObject.GetComponent<SpriteRenderer>().flipX = false;

        direction = Vector3.zero;//reset direction to zero
        velocity = Vector3.zero;//reset velocity to zero

        Move();

        //use input to test being damaged
        if (Input.GetKeyDown(KeyCode.F))
			TakeDamage(1);

        if(invincible == true)
        {
            if (timerInvul > invinTime)
            {
                invincible = false;
                timerInvul = 0;
            }
            else
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
        if(Input.GetMouseButtonDown(0) && timer > coolDown)
        {
            Attack();
            timer = 0;
        }
        //increment timer
        timer+= Time.deltaTime;
    }

    //method to spawn entity into the game
    public override void Spawn(Vector3 location, Vector3 rotation)
    {

    }

    //method to move the entity
    protected override void Move()
    {
        if (Input.GetKey(KeyCode.D))
        {
            direction.x += 1;
            pState = PlayerState.FacingRight;
        }
            

        if (Input.GetKey(KeyCode.A))
        {
            direction.x -= 1;
            pState = PlayerState.FacingLeft;
        }
            

        //calculate velocity from direction and speed times delta time so it is framerate independent
        velocity += direction.normalized * speed * Time.deltaTime;

        //update location by adding velocity
        transform.position = transform.position + velocity;

        //MoveCamera();
    }

    //method to handle when the entity dies
    protected override void Death()
    {
        GameManager.GM.ChangeHealth(0);//set health to zero if it was negative
        Debug.Log("You ded scrub");
    }

    //method to handle when the entity is attacked and no facing direction is specified
    public override void TakeDamage(int damageTaken)
    {
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

    //method to handle when the entity is attacked
    public void TakeDamage(int damageTaken, bool faceRight)
    {
        if(invincible == true)
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

    //method to handle when the entity attacks
    protected override void Attack()
    {
        Debug.Log("pew");

        //shoot a projectile
        GameObject orb = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0,0, transform.rotation.z)) as GameObject;

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

    private void Knockback(bool faceRight)
    {
        if(faceRight)
        {
            transform.position = new Vector3(transform.position.x - knockback, transform.position.y, transform.position.z);
            return;
        }
        transform.position = new Vector3(transform.position.x + knockback, transform.position.y, transform.position.z);
    }

    private void MoveCamera()
    {
        Vector3 location = transform.position;

        Camera cam = Camera.main;

        //Vector3 viewportPos = cam.WorldToViewportPoint(location);

        location.x = Mathf.Clamp(location.x, -3.35f, 3.5f);

        //location = cam.ViewportToWorldPoint(viewportPos);

        Camera.main.transform.position = new Vector3(location.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
    }
}
