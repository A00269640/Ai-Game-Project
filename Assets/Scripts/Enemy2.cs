using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    
    public Transform player;
    private Rigidbody2D rbody;
    Enemy_2Renderer isoRenderer;
    public float movementSpeed;
    public float maxSpeed = 1.9f;

    [SerializeField]
    private float range = 2f;

    //private bool detected;
   
    private Vector3 movement;
    private Vector3 randomMovement;
    private Vector3 homeMovement;
    public AudioSource running;

    [SerializeField]
    private bool collidingPlayer;

    [SerializeField]
    private bool playerInVision;

    [SerializeField]
    private bool playerOutOfRange;

    [SerializeField]
    private bool returning;

    private Vector3 spawnPoint;
    RaycastHit2D hit;
    
    private enum State
    {
        idle,
        attackingPlayer,
        chasing,
        returning
    }

    [SerializeField] private State state;

    void Start()
    {
        spawnPoint = transform.position;
        movementSpeed = Random.Range(1.3f, 1.8f);
        running = gameObject.GetComponent<AudioSource>();
        isoRenderer = GetComponentInChildren<Enemy_2Renderer>();
        rbody = this.GetComponent<Rigidbody2D>();
        gameObject.tag = "Enemy";
        //detected = false;
        state = State.idle;
        player = GameObject.FindWithTag("Player").transform;
        playerInVision = false;
        playerOutOfRange = false;
        returning = false;
        
    }

    void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        movement = direction;

        // deals with line of sight 
        hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 1 << LayerMask.NameToLayer(""));
        if (hit.collider.gameObject.tag == "Player" || hit.collider.gameObject.tag == "Hitbox")
        {
            Debug.Log("Player In Vision");
            playerInVision = true;
        }
        else
        {
            Debug.Log("Raycast Intercepted By " + hit.collider.name);
            playerInVision = false;
        }
            

        //If paused stops sound
        if (PlayerMovementController.paused == true)
        {
            running.Stop();
        }

        if (Vector3.Distance(player.position, transform.position) > range || playerInVision == false ) //if player is outside given range
        {
            if (returning == false)
            {
                isoRenderer.enemSetIdle(direction); //set animation to idle
                //stops running sound
                if (running.isPlaying)
                {
                    running.Stop();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        moveEnem(movement);
        //If self is above max speed sets velocity back to max
        if (rbody.velocity.magnitude > maxSpeed)
        {
            rbody.velocity = rbody.velocity.normalized * maxSpeed;
        }
    }

    //Finite State Machine For Movement/Attacking
    void moveEnem(Vector3 direction)
    {
        switch (state)
        {
            case State.idle:
                if (Vector3.Distance(player.position, transform.position) <= range && playerInVision == true)
                {
                    state = State.chasing;
                    playerOutOfRange = false;
                }
            break;
            case State.chasing:
                if (Vector3.Distance(player.position, transform.position) <= range && playerInVision == true) //if player comes closer than given range and is within vision
                {
                    if (collidingPlayer != true) //if not colliding with player
                    {
                        rbody.MovePosition((Vector3)transform.position + (direction * movementSpeed * Time.deltaTime)); //move towards player
                        isoRenderer.enemSetDirection(direction); //set animation direction
                    }
                    else if (collidingPlayer == true)
                    {
                        state = State.attackingPlayer;
                    }

                    //Plays running sound
                    if (!running.isPlaying)
                    {
                        running.Play();
                    }

                }
                else if (Vector3.Distance(player.position, transform.position) >= range)
                {
                    state = State.returning;
                }

                if(playerInVision == false && Vector3.Distance(player.position, transform.position) <= range)
                {
                    state = State.idle;
                }
            break;
            case State.attackingPlayer:
                if (collidingPlayer == true) //if colldiing with the player
                {
                    isoRenderer.enemSetAttackDirection(direction); //set attack animation direction
                    rbody.velocity = Vector3.zero; //stop moving while colliding
                }
                else
                {
                    isoRenderer.enemSetDirection(direction);
                    state = State.chasing;
                }
            break;
            case State.returning:
                returning = true;
                StartCoroutine(WalkAway(direction));       
            break;
        }
        
    }

    //On self collision with player sets a variable to become true
    void OnCollisionStay2D(Collision2D other)
    {         
        if (other.gameObject.tag.Equals("Player"))
        {
            collidingPlayer = true;
        }
    }

    //On self's exit of collision with player sets a variable to become false
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            collidingPlayer = false;
        }
    }

    //when player is out of range, will walk away to new location
    private IEnumerator WalkAway(Vector3 direction)
    {
            rbody.MovePosition((Vector3)transform.position + (-direction * movementSpeed * Time.deltaTime)); //move away from player
            isoRenderer.enemSetDirection(-direction);
            if (Vector3.Distance(player.position, transform.position) <= range && playerInVision == true)
            {
            state = State.chasing;
            }
            yield return new WaitForSeconds(4);
            returning = false;
            state = State.idle;
    }
}
