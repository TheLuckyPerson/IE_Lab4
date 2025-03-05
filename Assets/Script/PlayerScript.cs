using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.Image;

public class PlayerScript : MonoBehaviour
{
    public enum PlayerState
    {
        normal, dashing
    }

    [Serializable]
    public class InputStates
    {
        public float xDir;
        public bool jump;
        public bool dash;
        public GameObject spawnedObj;

        public InputStates(InputStates state)
        {
            this.xDir = state.xDir;
            this.jump = state.jump;
            this.dash = state.dash;
            this.spawnedObj = state.spawnedObj;
        }
    }

    public PlayerState state;
    [Header("Ground")]
    public List<Transform> groundDetectors;
    public List<Transform> allDetectors;
    public LayerMask groundLayer;
    private Rigidbody2D rb2d;
    [Header("Player values")]
    public float speed = 7f;
    public float jumpMag = 12f;
    public float facing = 1f;
    public float dashMag = 20f;
    public float dashDist = 2f;
    public float dashDistCovered = 0f;
    public bool isGrounded = false;
    public bool prevGroundState = false;
    public InputStates currentInputState;
    [Header("Dash")]
    public bool hasDashed = false;

    [Header("Shadow")]
    public PlayerScript shadowPrefab;
    public GameObject shadowHistoryPrefab;
    public PlayerScript original;
    public PlayerScript currentClone;
    public bool cloneExists = false;
    public float shadowTimer = 2f;
    public float shadowCountdown = 0f;
    public bool isShadow = false;
    public List<InputStates> inputStatesHistory = new List<InputStates>();
    public Vector2 startPos;
    public Vector2 offset = new Vector2(0, -.5f);
    public float cloneTimerLimit = 5f;
    public float currentCloneTimer = 0f;

    [Header("Animator")]
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [Header("Platform")]
    public LayerMask platformLayer;

    [Header("Sound")]
    public AudioSource jumpSound;
    public AudioSource spawnSound;
    public AudioSource dashSound;

    [Header("Shadow History")]
    Vector3 shadowHistoryOffset = new Vector3(0, -0.5f, 0);

    bool isPaused = false;
    bool stopRecording = false;
    Vector2 prevVelocity;
    public bool hasMoved = false;
    Vector2 parentVector = Vector2.zero;

    public bool canMove = true;
    public WinPanel winPanel;
    private bool hasTriggeredWin = false;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;
        isGrounded = IsGrounded(groundLayer);

        if (!isShadow)
        {  // record inputs
            currentInputState.xDir = Input.GetAxis("Horizontal");
            currentInputState.jump = Input.GetButtonDown("Jump");
            currentInputState.dash = Input.GetButtonDown("Dash");

            if ((currentInputState.xDir != 0 || currentInputState.jump || currentInputState.dash) && !hasMoved)
            {
                startPos = transform.position;
                hasMoved = true;
            }

            // spawn shadows
            if (!stopRecording && hasMoved)
            {
                shadowCountdown -= Time.deltaTime;

                if (shadowCountdown <= 0)
                {
                    GameObject g = Instantiate(shadowHistoryPrefab, transform.position + shadowHistoryOffset, Quaternion.identity);
                    currentInputState.spawnedObj = g.gameObject;
                    shadowCountdown = shadowTimer;
                }
                inputStatesHistory.Add(new InputStates(currentInputState));
            }

            DoCloneTimer();

        }
        else  // is a shadow clone
        {
            PauseClone();

            if (!isPaused)
            {
                if (inputStatesHistory.Count > 0)
                {
                    currentInputState = new InputStates(inputStatesHistory[0]);
                    inputStatesHistory[0].spawnedObj.SetActive(false);
                    inputStatesHistory.RemoveAt(0);
                }
                else
                {
                    if (original != null)
                    {
                        original.cloneExists = false;

                        foreach (InputStates inputs in original.inputStatesHistory)
                        {
                            inputs.spawnedObj.SetActive(true);
                        }
                    }
                    Destroy(gameObject);
                    return;
                }
            }
        }

        DoPlatformUpdates();


        if (state == PlayerState.normal)
        {
            DoMovement();
            spriteRenderer.flipX = facing == -1f;
            DoJump();
            // DoDash();

            if (!isGrounded && rb2d.linearVelocity.y <= 0)
            {
                animator.Play("Fall");
            }

            if (isGrounded && !prevGroundState)
            {
                animator.Play("Land");
            }
        }
        else if (state == PlayerState.dashing)
        {
            PerformDash();
        }

        // prevent shadows from making clones
        if (!isShadow)
            DoSpawn();

        if (isPaused)
        {
            rb2d.linearVelocity = Vector2.zero;
        }

        // reset button or death by falling off
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (transform.position.y < -5f)
        {
            KillPlayer();
        }

        prevGroundState = isGrounded;
    }

    void DoPlatformUpdates()
    {
        RaycastHit2D hit = IsGrounded(platformLayer);
        if (hit)
        {
            Rigidbody2D rbTarget = hit.transform.gameObject.GetComponent<Rigidbody2D>();
            parentVector = rbTarget.linearVelocity;
            rb2d.mass = .01f;
        }
        else
        {
            rb2d.mass = 1f;
            parentVector = Vector2.zero;
        }
    }

    void KillPlayer()
    {
        transform.position = startPos;
    }

    void DoCloneTimer()
    {
        currentCloneTimer += Time.deltaTime;

        if (currentCloneTimer > cloneTimerLimit)
        {
            currentCloneTimer = 0f;
            stopRecording = true;
        }
    }

    void PauseClone()
    {
        if (Input.GetButtonDown("Stop"))
        {
            if (isPaused)
            {
                isPaused = false;
                rb2d.linearVelocity = prevVelocity;
                rb2d.gravityScale = 1f;
                rb2d.mass = 1;
                spriteRenderer.color = new Color(1, 1, 1, 1);

            }
            else
            {
                isPaused = true;
                prevVelocity = rb2d.linearVelocity;
                rb2d.linearVelocity = Vector2.zero;
                rb2d.gravityScale = 0f;
                rb2d.mass = 1000;
                spriteRenderer.color = new Color(0,0,0,1);
            }
        }
    }

    void DoSpawn()
    {
        if (Input.GetButtonDown("Spawn") && hasMoved)
        {
            if (currentClone != null) {
                Destroy(currentClone.gameObject);
                currentClone = null;
            }
            spawnSound.Play();
            PlayerScript ps = Instantiate(shadowPrefab, startPos + offset, Quaternion.identity);
            currentClone = ps;
            for (int i = 0; i < inputStatesHistory.Count; i++)
            {
                ps.inputStatesHistory.Add(inputStatesHistory[i]);
                inputStatesHistory[i].spawnedObj.SetActive(true);
            }
            stopRecording = true;
            ps.original = this;
            ps.winPanel = winPanel;
            cloneExists = true;
        }
    }

    RaycastHit2D IsGrounded(LayerMask layer)
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(groundDetectors[0].position, Vector2.down, .05f, layer);
        foreach (Transform t in groundDetectors)
        {
            hit = Physics2D.Raycast(t.position, Vector2.down, .05f, layer);
            if (hit)
            {
                return hit;
            }
        }

        return hit;
    }

    void DoMovement()
    {
        float xDir = currentInputState.xDir;
        if (xDir != 0)
        {
            facing = xDir;
            if (isGrounded && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
                animator.Play("Run");
        }
        else if (isGrounded && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            animator.Play("Idle");
        }

        Vector2 additive = parentVector;
        if (additive.y == 0 || rb2d.linearVelocity.y != 0)
        {
            additive.y = rb2d.linearVelocity.y;
        }

        rb2d.linearVelocity = new Vector2(xDir * speed + additive.x, additive.y);

    }

    void DoJump()
    {
        if (currentInputState.jump && isGrounded)
        {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, jumpMag);
            animator.SetTrigger("Jump");
            jumpSound.Play();
        }
    }

    void PerformDash()
    {
        rb2d.linearVelocity = Vector2.right * facing * dashMag;
        dashDistCovered += dashMag * Time.deltaTime;

        if (dashDistCovered >= dashDist)
        {
            dashDistCovered = 0;
            state = PlayerState.normal;
        }

        animator.StopPlayback();
        animator.Play("Dash");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Finish" && !hasTriggeredWin)
        {
            hasTriggeredWin = true;
            winPanel.ShowWinScreen();
        }

        if (col.gameObject.tag == "Respawn" && !isShadow) {
            stopRecording = true;
            startPos = transform.position;

            while(inputStatesHistory.Count > 0) {
                inputStatesHistory[0].spawnedObj.SetActive(false);
                inputStatesHistory.RemoveAt(0);
            }

            shadowCountdown = 0;
            stopRecording = false;
            col.GetComponent<BoxCollider2D>().enabled = false;
            col.GetComponent<Animator>().SetTrigger("Trigger");
        }

        if (col.gameObject.tag == "Kill") {
            KillPlayer();
        }
    }
}
