using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a Tank Enemy
/// </summary>
public class TankEnemy : Entity {

	// variables 
	private SteeringForces steering;
	public CollisionHandler ch;
	public GameObject orb;
	private Vector3 force;
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
        bullet.GetComponent<Projectile>().parent = this.gameObject;
		bullet.transform.right = -1 * (steering.player.transform.position - transform.position).normalized;
	}

	public float GetAttack(){
		return attack;
	}
}