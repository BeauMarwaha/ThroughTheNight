using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Entity {

	// variables
	private SteeringForces steering;
	public CollisionHandler ch;
	public GameObject orb;
	private Vector3 force;
	private float projectileTimer;
	private float switchTimer;
	public int projectileCooldown;
	public int switchCooldown;
	public bool facingLeft;
	private EnemyMode em;

	public enum EnemyMode
	{
		Tank,
		Forward
	}

	// Use this for initialization
	protected override void Start () {
		em = EnemyMode.Tank;
		projectileTimer = projectileCooldown + 1;
		switchTimer = 0;
		steering = GetComponent<SteeringForces> ();
		ch = GameObject.Find ("GameManager").GetComponent<CollisionHandler> ();
		speed = 50f;
		attack = 1;
		health = 20f;
		direction = transform.forward;
		velocity = new Vector3(0,0,0);
	}
	
	// Update is called once per frame
	protected override void Update () {
		if (GameManager.GM.currentState != State.Message)
		{
			Death();
			SwitchModes ();
			Move();
			Rotate ();
			TakeDamage(1);
			if (em == EnemyMode.Tank) {
				if (projectileTimer > projectileCooldown)
				{
					projectileTimer = 0;
					Attack();
				}
				projectileTimer += Time.deltaTime;
			}

		}
	}

	//method to move the entity
	protected override void Move(){
		if (em == EnemyMode.Tank) {
			
		}

		if (em == EnemyMode.Forward) {
			if (Vector3.Dot (steering.player.transform.position, transform.right) > 0) {
				facingLeft = true;
				force += steering.Arrival (steering.player.transform.position , velocity, speed) * 300f;
			} else {
				facingLeft = false;
				force += steering.Arrival (steering.player.transform.position, velocity, speed) * 300f;
			}
			force = Vector3.ClampMagnitude (force, 100f);
			steering.ApplyForce (force);
			steering.UpdatePosition (velocity, direction);
			steering.SetTransform (direction);
		}
	}

	//method to handle when the entity dies
	protected override void Death(){
		// destroy the enemy when their health runs out
		if (health <= 0) {
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
		bullet.GetComponent<Projectile>().parent = this.gameObject;
		bullet.transform.right = -1 * (steering.player.transform.position - transform.position).normalized;
	}

	protected void Rotate(){
		if (facingLeft == true) {
			gameObject.GetComponent<SpriteRenderer> ().flipX = true;
		} 
		else {
			gameObject.GetComponent<SpriteRenderer> ().flipX = false;
		}
	}

	// method that will switch the attack modes of the enemey
	protected void SwitchModes(){
		// switch attack mode to that of a tank
		if (em == EnemyMode.Forward && switchTimer > switchCooldown) {
			switchTimer = 0;
			speed = 10f;
			em = EnemyMode.Tank;
		}

		// switch attack mode to that of a rushing enemy
		if (em == EnemyMode.Tank && switchTimer > switchCooldown) {
			switchTimer = 0;
			speed = 50f;
			projectileTimer = 0;
			em = EnemyMode.Forward;
		}

		// increment the timer for switching
		switchTimer += Time.deltaTime;

	}
}