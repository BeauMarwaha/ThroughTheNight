using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Dezmon Gilbert
/// Represents a Flying Enemy
/// </summary>
public class FlyingEnemy : Entity {

	// required for steering forces
	private SteeringForces steering;
	private Vector3 force; 

	// required for player bullet collision detection
	public CollisionHandler ch;
	private GameObject[] pBullets;

	// prefab required to create bullets
	public GameObject orb;

	// required for attack timing
	private float timer;
	public int cooldown;

	// Use this for initialization
	protected override void Start () {
		timer = cooldown + 1;
		ch = GameObject.Find ("GameManager").GetComponent<CollisionHandler> ();
		steering = GetComponent<SteeringForces> ();
		speed = 100f;
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
            TakeDamage(1);
            if (timer > cooldown)
            {
                timer = 0;
                Attack();
            }
            timer += Time.deltaTime;
        }
	}

	//method to move the entity using steering forces
	protected override void Move(){
		force += steering.WanderCircle(velocity, speed) * 50f;
		force = Vector3.ClampMagnitude (force, 200f);
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
		// check for collision between player bullet and game object
		pBullets = GameObject.FindGameObjectsWithTag("pBullet");
		if (pBullets.Length == 0) return;
		for (int i = 0; i < pBullets.Length; i++) {
			if (ch.AABBCollision (gameObject, pBullets [i])) {
				health -= damageTaken;
			}
		}
	}

	//method to handle when the entity attacks using projectiles 
	protected override void Attack(){
		// create bullet
		GameObject bullet = (GameObject)Instantiate(orb, transform.position,Quaternion.identity);

		// set the parent of the game object in the script
		bullet.GetComponent<Projectile>().parent = this.gameObject;

		// shoot the bullet toward the player
		bullet.transform.right = -1 * (steering.player.transform.position - transform.position).normalized;
	}
}