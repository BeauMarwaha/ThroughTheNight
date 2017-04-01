using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

	// Use this for initialization
	protected override void Start ()
    {
        //health = 100;
        speed = 5;
        attack = 5;
        Spawn(Vector3.zero, Vector3.zero);
	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        direction = Vector3.zero;//reset direction to zero

        Move();

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
        velocity = direction.normalized * speed * Time.deltaTime;

        //update location by adding velocity
        transform.position = transform.position + velocity;

    }

    //method to handle when the entity dies
    protected override void Death()
    {
        Debug.Log("You ded");
    }

    //method to handle when the entity is attacked
    protected override void Damaged()
    {
        //decrease health
        GameManager.GM.healthNum -= 5;

        //apply knockback
        velocity += Vector3(-5, 0, 0);
    }

    //method to handle when the entity attacks
    protected override void Attack()
    {

    }
}
