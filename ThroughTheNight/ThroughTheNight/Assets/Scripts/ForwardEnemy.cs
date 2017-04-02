using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a Forward Enemy
/// </summary>
public class ForwardEnemy :  Entity {

	// variables
	public CollisionHandler ch;
	private SteeringForces steering;
	private Vector3 force;
	public Vector3 dir;
	public bool facingLeft;

	// Use this for initialization
	protected override void Start () {
		steering = GetComponent<SteeringForces> ();
		ch = GameObject.Find ("GameManager").GetComponent<CollisionHandler> ();
		speed = 50f;
		attack = 1;
		health = 5f;
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
            TakeDamage(1);
            Attack();
        }

	}

	//method to move the entity
	protected override void Move(){
		if (Vector3.Dot (steering.player.transform.position, transform.right) > 0) {
			facingLeft = true;
			force += steering.Arrival (steering.player.transform.position , velocity, speed) * 700f;
		} else {
			facingLeft = false;
			force += steering.Arrival (steering.player.transform.position, velocity, speed) * 700f;
		}
		Debug.Log (Vector3.Dot (steering.player.transform.position, transform.right) );
		force = Vector3.ClampMagnitude (force, 500f);
		steering.ApplyForce (force);
		steering.UpdatePosition (velocity, direction);
		steering.SetTransform (direction);
	}

	//method to handle when the entity dies
	protected override void Death(){
		// destroy the game object 
		if (health <= 0) {
            // TO-DO: increment player currency


            // destroy enemy object
            GameManager.GM.DefeatEnemy();
			Destroy (gameObject);
		}
	}

	//method to handle when the entity is attacked
	public override void TakeDamage(int damageTaken){
		// check for collision between player bullet and game object
		GameObject[] pBullets = GameObject.FindGameObjectsWithTag("pBullet");
		if (pBullets.Length == 0) return;
		for (int i = 0; i < pBullets.Length; i++) {
			if (ch.AABBCollision (gameObject, pBullets [i])) {
				health -= damageTaken;
			}
		}
	}

	//method to handle when the entity attacks using projectiles
	// attacking by this enemy is handled in the CollisionHandler as this enemy doesn't use projectiles
	protected override void Attack(){}

	public float GetAttack(){
		return attack;
	}

	protected void Rotate(){
		if (facingLeft == true) {
			gameObject.GetComponent<SpriteRenderer> ().flipX = true;
		} 
		else {
			gameObject.GetComponent<SpriteRenderer> ().flipX = false;
		}
	}
}