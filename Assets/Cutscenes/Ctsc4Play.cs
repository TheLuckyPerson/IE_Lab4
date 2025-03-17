using System.Collections;
using UnityEngine;

public class Ctsc4Play : MonoBehaviour
{
    public static Ctsc4Play instance;
    [SerializeField] GameObject catPuppet;
    [SerializeField] GameObject thiefPuppet;
    [SerializeField] GameObject wizardPuppet;

    [SerializeField] GameObject coinBag;

    public bool slideAnimation = false;
    private float stopY = -3.6f;
    private float speed = 2f;

    public bool runTimeRight = false;
    public bool runTimeLeft = false;

    private Animator catAnimator;
    private Animator thiefAnimator;

    private bool flippedThief = false;
    public bool runAndJump = false;

    private bool playedJumpCat = false;
    private bool playedJumpThief = false;

    private bool wizardFlipping = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        catAnimator = catPuppet.GetComponent<Animator>();
        thiefAnimator = thiefPuppet.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (slideAnimation)
        {
            if (catPuppet.transform.position.y < stopY)
            {
                catPuppet.transform.Translate(Vector3.up * speed * Time.deltaTime);
            } 
            if (thiefPuppet.transform.position.y < stopY + 0.35f)
            {
                thiefPuppet.transform.Translate(Vector3.up * speed * Time.deltaTime);
            } 
        }

        if (runTimeRight)
        {
            if (catPuppet.transform.position.x < 3.35f)
            {
                catPuppet.transform.Translate(Vector3.right * 9 * Time.deltaTime);
                catAnimator.Play("Run");
            } else
            {
                Destroy(coinBag);
                SoundManager.instance.soundCoin();
                runTimeRight = false;
                Vector3 scale = catPuppet.transform.localScale;
                scale.x *= -1;
                catPuppet.transform.localScale = scale;
                runTimeLeft = true;
            }
        }

        if (runTimeLeft)
        {
            if (catPuppet.transform.position.x > -3.95f)
            {
                catPuppet.transform.Translate(Vector3.left * 9 * Time.deltaTime);

            } else
            {
                runTimeLeft = false;
                catAnimator.Play("Idle");
            }
        }

        if (runAndJump)
        {

            if (!flippedThief)
            {
                Vector3 scale = thiefPuppet.transform.localScale;
                scale.x *= -1;
                thiefPuppet.transform.localScale = scale;
                flippedThief = true;
            }

            if (catPuppet.transform.position.x > -8f)
            {
                catPuppet.transform.Translate(Vector3.left * 3 * Time.deltaTime);
                catAnimator.Play("Run");
            }
            else
            {
                catAnimator.Play("Jump");
                if (!playedJumpCat)
                {
                    SoundManager.instance.soundJump();
                    playedJumpCat = true;
                }
                
                catPuppet.transform.Translate(Vector3.left * 2 * Time.deltaTime);
                catPuppet.transform.Translate(Vector3.up * 3 * Time.deltaTime);
                StartCoroutine(WizardFlip());
            }

            if (thiefPuppet.transform.position.x > -8f)
            {
                thiefPuppet.transform.Translate(Vector2.left * 3 * Time.deltaTime);
                thiefAnimator.Play("Run");
            }
            else
            {
                thiefAnimator.Play("Jump");
                if (!playedJumpThief)
                {
                    SoundManager.instance.soundJump();
                    playedJumpThief = true;
                }
                thiefPuppet.transform.Translate(Vector3.left * 2 * Time.deltaTime);
                thiefPuppet.transform.Translate(Vector3.up * 3 * Time.deltaTime);
            }
        }
    }

    private IEnumerator WizardFlip()
    {
        if (wizardFlipping) yield break; 
        wizardFlipping = true;

        yield return new WaitForSeconds(1.5f);
        runAndJump = false;
        Vector3 scale = wizardPuppet.transform.localScale;
        scale.x *= -1;
        wizardPuppet.transform.localScale = scale;
        yield return new WaitForSeconds(1.5f);
        scale.x *= -1;
        wizardPuppet.transform.localScale = scale;
    }
}
