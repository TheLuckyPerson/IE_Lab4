using UnityEngine;

public class Ctsc1Play : MonoBehaviour
{
    public static Ctsc1Play instance;
    [SerializeField] GameObject catPuppet;

    public float speed;
    public float stopX;
    public bool catAnimation = false;
    private Animator animator;
    private bool stoppedOnce = false;

    public bool CatIsIdle => stoppedOnce;

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
       animator = catPuppet.GetComponent<Animator>();
       animator.Play("Run");
    }

    // Update is called once per frame
    void Update()
    {
        if (catAnimation)
        {
            if (catPuppet.transform.position.x < stopX)
            {
                catPuppet.transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
            else
            {
                if (!stoppedOnce)
                {
                    animator.Play("Idle");
                    stoppedOnce = true;
                }

            }
        }
    }
}
