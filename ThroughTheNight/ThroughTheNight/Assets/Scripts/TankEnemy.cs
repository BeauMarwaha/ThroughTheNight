using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Dezmon Gilbert
/// Represents a Tank Enemy
/// </summary>
public class TankEnemy : Entity {

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
		steering = GetComponent<SteeringForces> ();
		ch = GameObject.Find ("GameManager").GetComponent<CollisionHandler> ();
		speed = 0;
		attack = 1;
		health = 15f;
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

	//method to move the entity
	// tanks don't need to move as they will block the path of the player
	protected override void Move(){
	}

	//method to handle when the entity dies
	protected override void Death(){
		// destroy the enemy when their health runs out
		if (health <= 0) {
            // destroy enemy object
            GameManager.GM.DefeatEnemy();
            Destroy (gameObject);
		}
	}

	//method to handle when the entity is attacked
	public override void TakeDamage(int damageTaken)
    {


		// get an array of all bullets on the screen at a time
		pBullets = GameObject.FindGameObjectsWithTag("pBullet");

		// check if the array is empty
		if (pBullets.Length == 0) return;

		// loop through the player bullet array
		for (int i = 0; i < pBullets.Length; i++) {
			// check for collision between player bullet and game object
			if (ch.AABBCollision (gameObject, pBullets [i])) {

				//decrement health
				health -= damageTaken;
			}
		}
	}

	//method to handle when the entity attacks using projectiles
	protected override void Attack(){
        //play shot sound
        GameManager.GM.aSource.PlayOneShot(GameManager.GM.audioClips[8]);

        // create bullet
        GameObject bullet = (GameObject)Instantiate(orb, transform.position,Quaternion.identity);

		// set the parent of the game object in the script
        bullet.GetComponent<Projectile>().parent = this.gameObject;

		// shoot the bullet toward the player
		bullet.transform.right = -1 * (steering.player.transform.position - transform.position).normalized;
	}
}