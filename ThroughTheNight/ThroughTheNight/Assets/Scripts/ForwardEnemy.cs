using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Dezmon Gilbert
/// Represents a Forward Enemy
/// </summary>
public class ForwardEnemy :  Entity {

	// required for steering forces
	private SteeringForces steering;
	private Vector3 force; 

	// prefab required to create bullets
	public GameObject orb;

	// required for attack timing
	private float timer;
	public int cooldown;

	// required to help determine which way the sprite will face
	public bool facingLeft;

	// Use this for initialization
	protected override void Start () {
		steering = GetComponent<SteeringForces> ();
		speed = 50f;
		attack = 1;
		health = 10f;
		direction = transform.forward;
		velocity = new Vector3(0,0,0);
	}

	// Update is called once per frame
	protected override void Update () {
        if (GameManager.GM.currentState != State.Message && GameManager.GM.currentState != State.Over && GameManager.GM.currentState != State.Secret)
        {
            Death();
            Move();
            Rotate();
            Attack();
        }

	}

	//method to move the entity using steering forces
	protected override void Move(){
		// determine which side of the player the sprite is currently at by checking the dot product
		if (Vector3.Dot (steering.player.transform.position, transform.right) > 0) {
			facingLeft = true;
			force += steering.Arrival (steering.player.transform.position , velocity, speed) * 700f;
		} else {
			facingLeft = false;
			force += steering.Arrival (steering.player.transform.position, velocity, speed) * 700f;
		}
		force = Vector3.ClampMagnitude (force, 400f);
		steering.ApplyForce (force);
		steering.UpdatePosition (velocity, direction);
		steering.SetTransform (direction);
	}

	//method to handle when the entity dies
	protected override void Death(){
		// destroy the game object 
		if (health <= 0) {
            // destroy enemy object
            GameManager.GM.DefeatEnemy();
			Destroy (gameObject);
		}
	}

	//method to handle when the entity is attacked
	public override void TakeDamage(int damageTaken){
		//decrement health
		health -= damageTaken;
	}

	//method to handle when the entity attacks using projectiles
	// attacking by this enemy is handled in the CollisionHandler as this enemy doesn't use projectiles
	protected override void Attack(){}

	// will rotate the sprite to face the player
	protected void Rotate(){
		if (facingLeft == true) {
			gameObject.GetComponent<SpriteRenderer> ().flipX = true;
		} 
		else {
			gameObject.GetComponent<SpriteRenderer> ().flipX = false;
		}
	}
}