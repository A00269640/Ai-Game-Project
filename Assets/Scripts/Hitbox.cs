using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hitbox : PlayerMovementController
{

    public AudioSource weaponHit;
    public AudioSource crateHit;

    public GameObject ChestDestroyed;
    public GameObject DeadEnemy;
    public GameObject DeadFastEnemy;
    public GameObject HitEffect;
    public GameObject ChestHitEffect;

    public GameObject Enemy;



    public static event Action ScoreIncreased = delegate { };

    void Start()
    {
        AudioSource[] SourceArray = GetComponents<AudioSource>();
        weaponHit = SourceArray[0];
        crateHit = SourceArray[1];
        gameObject.tag = "Hitbox";
    }

    void Update()
    {
        
    }

    //Deals with collision of self and other objects
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If self collides with enemy destroys enemy and calls various gameplay functions
        if (collision.gameObject.tag.Equals("Enemy"))
        {   
            weaponHit.Play();
            Vector3 deathpos = collision.gameObject.transform.position;
            deathpos.z = 0;

            ScoreIncreased();
            //Instantiates a "dead enemy" gameobject at the previous collided object's position
            Instantiate(DeadEnemy, deathpos, Quaternion.identity);
            //Instantiates a hit particle effect gameobject at the previous collided object's position
            Instantiate(HitEffect, deathpos, Quaternion.identity);

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag.Equals("EnemyFast"))
        {
            weaponHit.Play();
            Vector3 deathpos = collision.gameObject.transform.position;
            deathpos.z = 0;

            ScoreIncreased();
            //Instantiates a "dead enemy" gameobject at the previous collided object's position
            Instantiate(DeadFastEnemy, deathpos, Quaternion.identity);
            //Instantiates a hit particle effect gameobject at the previous collided object's position
            Instantiate(HitEffect, deathpos, Quaternion.identity);

            Destroy(collision.gameObject);
        }

        //If self collides with chest destroys chest and calls various gameplay functions
        if (collision.gameObject.tag.Equals("Destructible"))
        {
            crateHit.Play();
            Vector3 destructPos = collision.gameObject.transform.position;
            destructPos.z = 1;
            ScoreIncreased();
            //Instantiates a "destroyed chest" gameobject at the previous collided object's position
            Instantiate(ChestDestroyed, destructPos, Quaternion.identity);
            //Instantiates a chest hit particle effect gameobject at the previous collided object's position
            Instantiate(ChestHitEffect, destructPos, Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }
}
