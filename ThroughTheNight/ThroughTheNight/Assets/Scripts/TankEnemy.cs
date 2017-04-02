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

	// prefab required to create bullets
	public GameObject orb;

	// required for attack timing
	private float timer;
	public int cooldown;

    //timer for flashing red when hit
    private float redLong = .25f;
    private float timerColor;
    private bool red;

	// Use this for initialization
	protected override void Start () {
        timerColor = 0;
        red = false;
		timer = cooldown + 1;
		steering = GetComponent<SteeringForces> ();
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
            if (timer > cooldown)
            {
                timer = 0;
                Attack();
            }
            timer += Time.deltaTime;
        }
		if(red)
        {
            if(timerColor > redLong)
            {
                timerColor = 0;
                red = false;
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                return;
            }
            timerColor += Time.deltaTime;
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
		//decrement health
		health -= damageTaken;

        //flash red
        red = true;
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
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