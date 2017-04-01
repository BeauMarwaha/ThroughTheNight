using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Entity {

	// variables
	public GameObject orb;
	public CollisionHandler ch;
	private SteeringForces steering;
	private Vector3 force;
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
        if (GameManager.GM.currentState != State.Message)
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

	//method to move the entity
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
			// TO-DO: increment player currency


			// destroy enemy object
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
	protected override void Attack(){
		// create bullet
		GameObject bullet = (GameObject)Instantiate(orb, transform.position,Quaternion.identity);
		bullet.transform.right = -1 * (steering.player.transform.position - transform.position).normalized;
	}

	public float GetAttack(){
		return attack;
	}
}