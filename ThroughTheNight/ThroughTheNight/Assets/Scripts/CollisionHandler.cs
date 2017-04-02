using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Beau Marwaha
/// Handles checking for collisions.
/// </summary>
public class CollisionHandler : MonoBehaviour {

    //attributes
    private GameObject player;
    private GameObject secret;
    private GameObject[] enemies;
    private GameObject[] pBullets; //player bullets
    private GameObject[] eBullets; //enemy bullets
    private GameObject[] hearts; //little restorative hearts

    // Use this for initialization
    void Start()
    {
        //initialize attributes
        pBullets = GameObject.FindGameObjectsWithTag("pBullet");
        eBullets = GameObject.FindGameObjectsWithTag("eBullet");
        hearts = GameObject.FindGameObjectsWithTag("Heart");
    }

    // Update is called once per frame
    void Update()
    {
        //Update player and enemies refs
        player = GameObject.FindGameObjectWithTag("Player");
        secret = GameObject.FindGameObjectWithTag("Secret");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //Check for collisions between the player and all enemies
        PlayerEnemyCollisionCheck();

        //Check for collisions between the player and all enemy bullets
        PlayerEBulletCollisionCheck();

        //Check for collisions between all enemies and all player bullets
        EnemyPBulletCollisionCheck();

        //check for collsions between enemy and player bullets
        PBulletEBulletCollisionCheck();

        //check for collisions between player and hearts
        HeartPlayerCollisionCheck();

        SecretPlayerCollisionCheck();
    }

    /// <summary>
	/// Checks if two game objects are colliding using AABB collision.
	/// </summary>
	/// <returns><c>true</c>, if collision was AABBed, <c>false</c> otherwise.</returns>
	/// <param name="obj1">Obj1.</param>
	/// <param name="obj2">Obj2.</param>
	public bool AABBCollision(GameObject obj1, GameObject obj2)
    {
        //get the sprtie info scripts from each game object which hold corrected bounds of the sprite renderers
        SpriteInfo info1 = obj1.GetComponent<SpriteInfo>();
        SpriteInfo info2 = obj2.GetComponent<SpriteInfo>();
        
        //check for AABB collision
        if (info1.GetMinX() < info2.GetMaxX() &&
            info1.GetMaxX() > info2.GetMinX() &&
            info1.GetMinY() < info2.GetMaxY() &&
            info1.GetMaxY() > info2.GetMinY())
        {
            //Debug.Log("Colliding");
            return true;
        }

        //if they are not colliding return false
        return false;
    }

    /// <summary>
    /// Checks if two game objects are colliding using circle collision.
    /// </summary>
    /// <returns><c>true</c>, if collision was detected, <c>false</c> otherwise.</returns>
    /// <param name="obj1">Obj1.</param>
    /// <param name="obj2">Obj2.</param>
    public bool CircleCollision(GameObject obj1, GameObject obj2)
    {
        //get the sprtie info scripts from each game object which hold corrected bounds of the sprite renderers
        SpriteInfo info1 = obj1.GetComponent<SpriteInfo>();
        SpriteInfo info2 = obj2.GetComponent<SpriteInfo>();

        //distance between centers
        Vector3 distance = info2.Center() - info1.Center();
        float dist = distance.magnitude * distance.magnitude;

        //check for AABB collision
        if ((info1.GetRadiusBullet() + info2.GetRadiusBullet())* (info1.GetRadiusBullet() + info2.GetRadiusBullet()) > dist)
        {
            //Debug.Log("Colliding");
            return true;
        }

        //if they are not colliding return false
        return false;
    }

    /// <summary>
    /// Check for AABB collisions between the player and all enemies
    /// </summary>
    private void PlayerEnemyCollisionCheck()
    {
        //for each enemy
        for (int i = 0; i < enemies.Length; i++)
        {
            //check for collision
            if (enemies[i].activeSelf && AABBCollision(player, enemies[i]))
            {
                //get the dot product of the players right vector and the enemy
                float dot = Vector3.Dot(player.transform.right, enemies[i].transform.position);
                //Debug.Log("Dot product: " + dot);
                if(dot < 0)
                {
                    //if colliding have the player take damage
                    player.GetComponent<Player>().TakeDamage(enemies[i].GetComponent<Entity>().attack, false);
                }
                else
                {
                    //if colliding have the player take damage
                    player.GetComponent<Player>().TakeDamage(enemies[i].GetComponent<Entity>().attack, true);
                }
                
            }
        }
    }

    /// <summary>
    /// Check for AABB collisions between all enemies and all player bullets
    /// </summary>
    private void EnemyPBulletCollisionCheck()
    {
        //update bullet list
        pBullets = GameObject.FindGameObjectsWithTag("pBullet");

        //check all bullets
        foreach (GameObject bullet in pBullets)
        {
            //check all enemies
            for (int i = 0; i < enemies.Length; i++)
            {
                //check for collision
                if (enemies[i].activeSelf && AABBCollision(bullet, enemies[i]))
                {
                    enemies[i].GetComponent<Entity>().TakeDamage(player.GetComponent<Entity>().attack);
                    bullet.GetComponent<Projectile>().Hit();
                }
            }
        }

    }

    /// <summary>
    /// Check for AABB collisions between the player and all enemy bullets
    /// </summary>
    private void PlayerEBulletCollisionCheck()
    {
        //update bullet list
        eBullets = GameObject.FindGameObjectsWithTag("eBullet");

        //check all bullets
        foreach (GameObject bullet in eBullets)
        {
            if (AABBCollision(player, bullet))
            {             
                //get the dot product of the players right vector and the enemy
                float dot = Vector3.Dot(player.transform.right, bullet.transform.position);
                //Debug.Log("Dot product: " + dot);
                if (dot < 0)
                {
                    //if colliding have the player take damage
                    player.GetComponent<Player>().TakeDamage(bullet.GetComponent<Projectile>().parent.GetComponent<Entity>().attack, false);
                }
                else
                {
                    //if colliding have the player take damage
                    player.GetComponent<Player>().TakeDamage(bullet.GetComponent<Projectile>().parent.GetComponent<Entity>().attack, true);
                }

                bullet.GetComponent<Projectile>().Hit();
            }
        }

    }

    /// <summary>
    /// Checks using circle collision whether player and enemy bullets are colliding
    /// </summary>
    private void PBulletEBulletCollisionCheck()
    {
        //update player bullet list
        pBullets = GameObject.FindGameObjectsWithTag("pBullet");

        //update enemy bullet list
        eBullets = GameObject.FindGameObjectsWithTag("eBullet");

        //check all bullets
        foreach (GameObject bullet in eBullets)
        {
            foreach(GameObject b in pBullets)
            {
                if (CircleCollision(b, bullet))//AABBCollision(b, bullet))
                {
                    //if the two bullets collide, destroy both of them
                    bullet.GetComponent<Projectile>().Hit();
                    b.GetComponent<Projectile>().Hit();
                }
            }
            
        }
    }

    /// <summary>
    /// Checks using AABB collision whether the player is colliding with any hearts
    /// </summary>
    private void HeartPlayerCollisionCheck()
    {
        //update bullet list
        hearts = GameObject.FindGameObjectsWithTag("Heart");

        //check all bullets
        foreach (GameObject h in hearts)
        {
            if (AABBCollision(player, h))
            {
                //if colliding have the player take damage
                player.GetComponent<Player>().Restore();

                Destroy(h);
            }
        }
    }

    /// <summary>
    /// Checks using AABB collision whether the player is colliding with the secret, nope, not telling you what it is
    /// </summary>
    private void SecretPlayerCollisionCheck()
    {
        if (AABBCollision(player, secret) && Input.GetKeyDown(KeyCode.E))
        {
            //if colliding and user pressed E activate secret
            GameManager.GM.currentState = State.Secret;
        }
    }

}
