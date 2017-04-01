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
    private GameObject[] enemies;
    private GameObject[] pBullets; //player bullets
    private GameObject[] eBullets; //enemy bullets

    // Use this for initialization
    void Start()
    {
        //initialize attributes
        
        pBullets = GameObject.FindGameObjectsWithTag("pBullet");
        eBullets = GameObject.FindGameObjectsWithTag("eBullet");
    }

    // Update is called once per frame
    void Update()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //Check for collisions between the player and all enemies
        PlayerEnemyCollisionCheck();

        //Check for collisions between the player and all enemy bullets
        PlayerEBulletCollisionCheck();

        //Check for collisions between all enemies and all player bullets
        EnemyPBulletCollisionCheck();

        //check for collsions between enemy and player bullets
        PBulletEBulletCollisionCheck();

        if(player != null)
        {
            SpriteInfo info1 = player.GetComponent<SpriteInfo>();
            Debug.DrawLine(new Vector3(info1.GetMinX(), info1.GetMinY(), 0), new Vector3(info1.GetMaxX(), info1.GetMinY(), 0),Color.red);
            Debug.DrawLine(new Vector3(info1.GetMinX(), info1.GetMinY(), 0), new Vector3(info1.GetMinX(), info1.GetMaxY(), 0), Color.red);
            Debug.DrawLine(new Vector3(info1.GetMaxX(), info1.GetMinY(), 0), new Vector3(info1.GetMaxX(), info1.GetMaxY(), 0), Color.red);
            Debug.DrawLine(new Vector3(info1.GetMinX(), info1.GetMaxY(), 0), new Vector3(info1.GetMaxX(), info1.GetMaxY(), 0), Color.red);
        }
        
        //Debug.Log(info1.GetMinX() + " " + info1.GetMaxX() + " " + info1.GetMinY() + " " + info1.GetMaxY());
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

        //Debug.Log(info1.GetMinX() + " " + info1.GetMaxX() + " " + info1.GetMinY() + " " + info1.GetMaxY());

        //check for AABB collision
        if (info1.GetMinX() < info2.GetMaxX() &&
            info1.GetMaxX() > info2.GetMinX() &&
            info1.GetMinY() < info2.GetMaxY() &&
            info1.GetMaxY() > info2.GetMinY())
        {
            Debug.Log("Colliding");
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
            Debug.Log("Colliding");
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
                Debug.Log("Dot product: " + dot);
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
                Debug.Log("Dot product: " + dot);
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


}
