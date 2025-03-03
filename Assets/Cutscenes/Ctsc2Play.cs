using UnityEngine;

public class Ctsc2Play : MonoBehaviour
{
    public static Ctsc2Play instance;
    [SerializeField] GameObject catPuppet;
    [SerializeField] Camera mainCamera;

    public float speed = 2f;
    public float stopX;
    public bool catAnimation = false;

    private bool isFlipping = false;
    private float flipTimer = 0f;
    private float flipInterval = 0.5f; 

    private Vector3 cameraStartPos;
    private Vector3 cameraTargetPos;

    public bool isPanningCamera = false;
    public float cameraPanSpeed = 2f;

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

    void Start()
    {
        cameraStartPos = mainCamera.transform.position;
        cameraTargetPos = cameraStartPos + new Vector3(0, -7, 0);
    }

    void Update()
    {
        if (catAnimation)
        {
            FlipBackAndForth();
            RiseUp();
        }

        if (isPanningCamera)
        {
            mainCamera.transform.position = Vector3.MoveTowards(
                mainCamera.transform.position,
                cameraTargetPos,
                cameraPanSpeed * Time.deltaTime
            );

            if (Vector3.Distance(mainCamera.transform.position, cameraTargetPos) < 0.1f)
            {
                isPanningCamera = false;
            }
        }
    }

    private void FlipBackAndForth()
    {
        flipTimer += Time.deltaTime;

        if (flipTimer >= flipInterval)
        {
            flipTimer = 0f;
            isFlipping = !isFlipping;
            Vector3 scale = catPuppet.transform.localScale;
            scale.x *= -1; 
            catPuppet.transform.localScale = scale;
        }
    }

    private void RiseUp()
    {
        catPuppet.transform.position += new Vector3(0, speed * Time.deltaTime * 0.5f, 0);
    }
}
