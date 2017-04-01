using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    //fields
    public float knockback;
    public GameObject bulletPrefab;
    //total amount rotated
    private float totalRotation;
    //fields for shooting cooldown
    private int timer;
    public int coolDown; //time between shots //25

    //properties
    public float TotalRot
    {
        get { return totalRotation; }
    }

    // Use this for initialization
    protected override void Start ()
    {
        //instantiate timer to be higher than cooldown so that you can fire immediately
        timer = coolDown + 1;

        speed = 5;
        attack = 5;
        Spawn(Vector3.zero, Vector3.zero);
	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        direction = Vector3.zero;//reset direction to zero
        velocity = Vector3.zero;//reset velocity to zero

        Move();

        //use input to test being damaged
        if (Input.GetKeyDown(KeyCode.F))
			TakeDamage(1);

        //check for if the player's health is gone
        if (GameManager.GM.healthNum <= 0)
        {
            Death();
        }

        //if the player clicks, fire a projectile at where the mouse is currently
        if(Input.GetMouseButtonDown(0) && timer > coolDown)
        {
            Attack();
        }
        //increment timer
        timer++;
    }

    //method to spawn entity into the game
    public override void Spawn(Vector3 location, Vector3 rotation)
    {

    }

    //method to move the entity
    protected override void Move()
    {
        if (Input.GetKey(KeyCode.D))
            direction.x += 1;

        if (Input.GetKey(KeyCode.A))
            direction.x -= 1;

        //calculate velocity from direction and speed times delta time so it is framerate independent
        velocity += direction.normalized * speed * Time.deltaTime;

        //update location by adding velocity
        transform.position = transform.position + velocity;

    }

    //method to handle when the entity dies
    protected override void Death()
    {
        GameManager.GM.ChangeHealth(0);//set health to zero if it was negative
        Debug.Log("You ded scrub");
    }

    //method to handle when the entity is attacked
	protected override void TakeDamage(int damageTaken)
    {
        //decrease health
        GameManager.GM.ChangeHealth(-damageTaken);

        //apply knockback
        transform.position = new Vector3(transform.position.x - knockback, transform.position.y, transform.position.z);
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
        //use arctan to determine angle betweeen clock hand and mouse
        float mouseRot = Mathf.Atan2(mousePos.y, mousePos.x);
        //convert angle from rads to deg and add 180 to compensate for being backwards
        mouseRot = Mathf.Rad2Deg * mouseRot + 180f;
        //set the rotation angle as an Euler angle
        orb.transform.rotation = Quaternion.Euler(0, 0, mouseRot);

    }
}
