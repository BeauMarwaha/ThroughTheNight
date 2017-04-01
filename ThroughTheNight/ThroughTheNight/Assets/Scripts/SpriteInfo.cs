using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Beau Marwaha
/// Contatins methods that retrieve sprite info
/// </summary>
public class SpriteInfo : MonoBehaviour {
    //gets the minimum x of the sprite object
    public float GetMinX()
    {
        return this.GetComponent<SpriteRenderer>().bounds.min.x;
    }

    //gets the maximum x of the sprite object
    public float GetMaxX()
    {
        return this.GetComponent<SpriteRenderer>().bounds.max.x;
    }

    //gets the minimum y of the sprite object
    public float GetMinY()
    {
        return this.GetComponent<SpriteRenderer>().bounds.min.y;
    }

    //gets the maximum y of the sprite object
    public float GetMaxY()
    {
        return this.GetComponent<SpriteRenderer>().bounds.max.y;
    }

    //gets the radius of the bullet sprite objects
    public float GetRadiusBullet()
    {
        return this.GetComponent<SpriteRenderer>().bounds.extents.y* .75f;
    }

    //gets the x radius of the player and creature sprites
    public float GetRadius()
    {
        return this.GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    //gets the x radius of the player and creature sprites
    public Vector3 Center()
    {
        return this.GetComponent<SpriteRenderer>().bounds.center;
    }
}
