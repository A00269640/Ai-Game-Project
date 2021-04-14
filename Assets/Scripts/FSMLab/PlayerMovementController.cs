using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;
using UnityEngine.SceneManagement;

public class PlayerMovementController : MonoBehaviour
{

    public float movementSpeed = 2.5f;
    CharacterRenderer isoRenderer;

    public GameObject CharacterRenderer;
    public GameObject attackIcon;
    public GameObject Gain;
    public GameObject GainST;

    public GameObject PauseMenu;

    public AudioSource run;
    public AudioSource swing;
    public AudioSource hurt;

    Rigidbody2D rbody;


    [SerializeField]
    public GameObject HitboxN;
    public GameObject HitboxS;
    public GameObject HitboxE;
    public GameObject HitboxW;
    public GameObject HitboxA;


    public int maxHealth = 100;

    [SerializeField]
    public int health = 100;
    public bool isInvincible;
    public bool canAttack;
    public float invincibilityDurationSeconds;
    public float flashDuration;
    public float canNotAttackDuration;
    public bool scoreAchieved;
    public float scoreNeeded;
    public static bool paused;
    public int scoreCurrent;
    public static Vector3 currentSpeed;

    //State initialization
    private enum State
    {
        normal,
        attacking,
    }

    Animator otherAnimator;

    [SerializeField] private State state;

    public HealthBar healthBar;

    private void Start()
    {
        AudioSource[] SourceArray = GetComponents<AudioSource>();

        run = SourceArray[0];
        swing = SourceArray[1];
        hurt = SourceArray[2];

        HitboxN.SetActive(false);
        HitboxS.SetActive(false);
        HitboxE.SetActive(false);
        HitboxW.SetActive(false);
        HitboxA.SetActive(false);
        healthBar.SetMaxHealth(health);
        attackIcon.SetActive(false);
        Gain.SetActive(true);
        GainST.SetActive(false);
        PauseMenu.SetActive(false);

        scoreAchieved = false;
        paused = false;

        //Sets score required per level
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            scoreNeeded = 1000;
        }
        else if (SceneManager.GetActiveScene().name == "Level1")
        {
            scoreNeeded = 6000;
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            scoreNeeded = 8000;
        }

        movementSpeed = 2.5f;
    }

    private void Awake()
    {
        otherAnimator = CharacterRenderer.GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<CharacterRenderer>();
        gameObject.tag = "Player";
        state = State.normal;
        isInvincible = false;
        invincibilityDurationSeconds = 2f;
        flashDuration = 0.15f;
        canAttack = true;
        canNotAttackDuration = 1f;

    }

    void Update()
    {
        currentSpeed = rbody.velocity;

        scoreCurrent = GlobalVars.score;

        healthBar.SetHealth(health);

        //Part of score functionality
        if (scoreAchieved == false)
        {
            if (GlobalVars.score >= scoreNeeded)
            {
                Gain.SetActive(false);
                GainST.SetActive(true);
                scoreAchieved = true;
            }
        }


        //Following consists of movement and attacking inputs

        //Pause Key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused == false)
            {
                Time.timeScale = 0;               
                PauseMenu.SetActive(true);
                paused = true;
                
            }
            else if (paused == true)
            {
                Time.timeScale = 1;
                PauseMenu.SetActive(false);
                paused = false;
            }
        }

        //Play running sound
        if (Input.GetButtonDown("Horizontal") && paused == false || Input.GetButtonDown("Vertical") && paused == false)
        {
            run.Play();
        }
        else if (!Input.GetButton("Horizontal") && !Input.GetButton("Vertical") && run.isPlaying)
        {
            run.Stop();
        }

        //Requirements to enable hitboxes
        if(Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && isInvincible != true && canAttack == true)
        {

            HitboxA.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && isInvincible != true && canAttack == true)
        {

            HitboxN.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W) && isInvincible != true && canAttack == true)
        {
  
            HitboxS.SetActive(true);

        }

        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && isInvincible != true && canAttack == true)
        {

            HitboxE.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && isInvincible != true && canAttack == true)
        {

            HitboxW.SetActive(true);
        }

        //Plays swing sound and deals with further attack functionality
        if (Input.GetMouseButtonDown(0) && isInvincible != true && canAttack == true && paused == false)
        {
            swing.Play();
            attackIcon.SetActive(true);
            StartCoroutine("canNotAttack");
        }

        //Sets self velocity to 0 when key is released
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            rbody.velocity = Vector2.zero;
        }

        //Deals with setting walking and attacking animation directions, depending on current state
        switch (state)
        {
            case State.normal:
            Vector2 currentPos = rbody.position;

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
            inputVector = Vector2.ClampMagnitude(inputVector, 1);

            Vector2 movement = inputVector * movementSpeed;
            Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;

            if (state != State.attacking)
            {
                isoRenderer.SetDirection(movement);
                rbody.MovePosition(newPos);
            }



            if (state == State.normal)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (isInvincible != true)
                    {
                        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
                        Vector3 mouseDir = (mousePosition - transform.position).normalized;
                        float attackOffset = 2f;
                        Vector3 attackPosition = transform.position + mouseDir * attackOffset;
                        Debug.Log(attackPosition);
                        state = State.attacking;
                        isoRenderer.SetAttackDirection(mouseDir);
                    }
                    StartCoroutine("OnCompleteAttackAnimation");
                }
            }
            
            break;
            case State.attacking:
            break;
            
        }
    }

    //After attack animation finishes, sets state to normal and deactivates hitboxes
    IEnumerator OnCompleteAttackAnimation()
    {
        while (otherAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        yield return null;


        state = State.normal;    
        HitboxN.SetActive(false);
        HitboxS.SetActive(false);
        HitboxE.SetActive(false);
        HitboxW.SetActive(false);
        HitboxA.SetActive(false);
    }

    //Deals with collisions of various types
    private void OnCollisionStay2D(Collision2D other)
    {
        //Damages player if collision with enemy
        if (other.gameObject.tag.Equals("Enemy"))
        {
            if (isInvincible) return;

            if (health > 0 && isInvincible != true)
            {
                health = health - 10;
                StartCoroutine(BecomeTemporarilyInvincible());
                StartCoroutine(TempFlash());
                hurt.Play();
            }
            else
            {
                gameObject.SetActive(false);
                SceneManager.LoadScene("DeathScreen");
                hurt.Play();
            }
        }

        if (other.gameObject.tag.Equals("EnemyFast"))
        {
            if (isInvincible) return;

            if (health > 0 && isInvincible != true)
            {
                health = health - 10;
                StartCoroutine(BecomeTemporarilyInvincible());
                StartCoroutine(TempFlash());
                hurt.Play();
            }
            else
            {
                gameObject.SetActive(false);
                SceneManager.LoadScene("DeathScreen");
                hurt.Play();
            }
        }
        //Loads scene on collision, is score is acquired
        if (other.gameObject.tag.Equals("Level1"))
        {
            if (scoreAchieved == true)
            {
                SceneManager.LoadScene("Level1");
            }
            else
            {
                return;
            }
        }
        //Loads scene on collision, is score is acquired
        if (other.gameObject.tag.Equals("Level2"))
        {
            if (scoreAchieved == true)
            {
                SceneManager.LoadScene("Level2");
            }
            else
            {
                return;
            }
        }
        //Loads scene on collision, is score is acquired
        if (other.gameObject.tag.Equals("End"))
        {
            if (scoreAchieved == true)
            {
                SceneManager.LoadScene("CreditsScene");
            }
            else
            {
                return;
            }
        }
    }

    //Invincibility frames
    private IEnumerator BecomeTemporarilyInvincible()
    {
        isInvincible = true;
        attackIcon.SetActive(true);

        yield return new WaitForSeconds(invincibilityDurationSeconds);

        isInvincible = false;
        attackIcon.SetActive(false);
    }

    //Disallows player from attacking for x seconds
    private IEnumerator canNotAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(canNotAttackDuration);
        canAttack = true;
        attackIcon.SetActive(false);
    }

    //"Hides" player render when called
    void HideRender()
    {
        GameObject.Find("PlayerRender").transform.localScale = new Vector3(0, 0, 0);
    }

    //"Shows" player render when called
    public void ShowRender()
    {
        GameObject.Find("PlayerRender").transform.localScale = new Vector3(1, 1, 1);
    }

    //Calls render functions intermittently to give effect of player flashing after being hit, to give visual feedback to invincibility frames
    private IEnumerator TempFlash()
    {
        for (int i = 0; i <= 5; i++)
        {
            HideRender();

            yield return new WaitForSeconds(flashDuration);

            ShowRender();

            yield return new WaitForSeconds(flashDuration);
        }
        HideRender();

        yield return new WaitForSeconds(flashDuration);

        ShowRender();
    }
}
