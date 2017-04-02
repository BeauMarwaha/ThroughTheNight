using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Dezmon Gilbert & Kat Weis
/// Represents a Boss enemy.
/// </summary>
public class BossEnemy : Entity {

	// required for steering forces
	private SteeringForces steering;
	private Vector3 force; 

	// prefab required to create bullets
	public GameObject orb;

	// required to keep track of attack timing and enemy type switching
	private float projectileTimer;
	private float switchTimer;
	public int projectileCooldown;
	public int switchCooldown;

	// required to help determine which way the sprite will face
	public bool facingLeft;

	// enum that will handle the current attack mode of the enemy
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
		speed = 50f;
		attack = 1;
		health = 65f;
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
		// the boss in tank mode does not move
		if (em == EnemyMode.Tank) {
			
		}

		// the boss will have the same movement as a forward enemy
		if (em == EnemyMode.Forward) {
			if (Vector3.Dot (steering.player.transform.position, transform.right) > 0) {
				facingLeft = true;
				force += steering.Arrival (steering.player.transform.position , velocity, speed) * 300f;
			} else {
				facingLeft = false;
				force += steering.Arrival (steering.player.transform.position, velocity, speed) * 300f;
			}
			force = Vector3.ClampMagnitude (force, 300f);
			steering.ApplyForce (force);
			steering.UpdatePosition (velocity, direction);
			steering.SetTransform (direction);
		}
	}

	//method to handle when the entity dies
	protected override void Death(){
		// destroy the enemy when their health runs out
		if (health <= 0) {
			// call defeat to decrement the remaining numbers of enemies and win the game
			GameManager.GM.DefeatEnemy();

			// destroy enemy object
			Destroy (gameObject);
		}
	}

	//method to handle when the entity is attacked
	public override void TakeDamage(int damageTaken){
		//decrement health
		health -= damageTaken;
	}

    /// <summary>
    /// method to handle when the boss attacks using three projectiles
    /// </summary>
    protected override void Attack()
    {
        //plays sound for shooting
        GameManager.GM.aSource.PlayOneShot(GameManager.GM.audioClips[8]);

        // create 3 bullets
        GameObject bullet = (GameObject)Instantiate(orb, transform.position,Quaternion.identity);
        GameObject bullet1 = (GameObject)Instantiate(orb, new Vector3(transform.position.x, transform.position.y*1.5f, transform.position.z), Quaternion.identity);
        GameObject bullet2 = (GameObject)Instantiate(orb, new Vector3(transform.position.x, transform.position.y * .5f, transform.position.z), Quaternion.identity);

        //set the parent of the bullets to this enemy
        bullet.GetComponent<Projectile>().parent = this.gameObject;
        bullet1.GetComponent<Projectile>().parent = this.gameObject;
        bullet2.GetComponent<Projectile>().parent = this.gameObject;

        //set the right vector of the bullet so that it moves correctly
        bullet.transform.right = -1 * (steering.player.transform.position - transform.position).normalized;
        bullet1.transform.right = -1 * (steering.player.transform.position - transform.position).normalized;
        bullet2.transform.right = -1 * (steering.player.transform.position - transform.position).normalized;
    }

	// will rotate the sprite to face the player
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