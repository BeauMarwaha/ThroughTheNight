using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : Entity {
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
		speed = 10f;
		attack = 1;
		health = 8f;
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

            //transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            //TakeDamage ();
            if (timer > cooldown)
            {
                timer = 0;
                Attack();
            }
            timer += Time.deltaTime;
        }
		
	}

	public override void Spawn(Vector3 location, Vector3 rotation){
	
	}


	//method to spawn entity into the game
	public GameObject Spawn(GameObject prefab, Vector3 location, Vector3 rotation){
		return (GameObject)Instantiate (prefab, location, Quaternion.Euler(rotation));
	}

	//method to move the entity
	protected override void Move(){
		// move closer to the player up to a certain distance
		if (steering.DistToPlayer() > 5f) {
			steering.SeekPlayer (velocity, speed);
			force += Vector3.ClampMagnitude (force, 10f);
			steering.ApplyForce (force);
			steering.UpdatePosition (velocity, direction);
			steering.SetTransform (direction);
		}
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

	//method to handle when the entity attacks
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