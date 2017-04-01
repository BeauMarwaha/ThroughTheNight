using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    //fields
    protected float speed;
    protected float attack;
    protected float health;
    protected Vector3 velocity;
    protected Vector3 direction;

    // Use this for initialization
    abstract void Start();

    // Update is called once per frame
    abstract void Update();

    //method to spawn entity into the game
    abstract void Spawn(Vector3 location, Vector3 rotation);

    //method to move the entity
    abstract void Move();

    //method to handle when the entity dies
    abstract void Death();

    //method to handle when the entity is attacked
    abstract void Damaged();

    //method to handle when the entity attacks
    abstract void Attack();
}
