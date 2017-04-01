using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    //fields
    public float knockback;

	// Use this for initialization
	protected override void Start ()
    {
        health = 100;
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
            Damaged();

        //check for if the player's health is gone
        if (GameManager.GM.healthNum <= 0)
        {
            Death();
        }
    }

    //method to spawn entity into the game
    protected override void Spawn(Vector3 location, Vector3 rotation)
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
    protected override void Damaged()
    {
        //decrease health
        GameManager.GM.ChangeHealth(-5);

        //apply knockback
        transform.position = new Vector3(transform.position.x - knockback, transform.position.y, transform.position.z);
    }

    //method to handle when the entity attacks
    protected override void Attack()
    {

    }
}
