using UnityEngine;

public class MovingPlatform : Buttonable
{
    public Vector2 goToFromStart;
    public float speed;
    private Vector2 startPos;
    private Vector2 endPos;
    private Vector3 gotoPos;
    private Vector2 fromPos;
    private float dir = 1;
    private Rigidbody2D rb2d;
    public bool isOn;

    public void Start()
    {
        startPos = transform.position;
        endPos = goToFromStart + startPos;
        gotoPos = endPos;
        fromPos = startPos;
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if (isOn) {
            rb2d.linearVelocity = goToFromStart.normalized * speed * dir;
            if (Vector3.Distance(fromPos, transform.position) >= Vector3.Distance(fromPos, gotoPos)) {
                if (dir == -1) {
                    dir = 1;
                    gotoPos = endPos;
                    fromPos = startPos;
                } else {
                    dir = -1;
                    gotoPos = startPos;
                    fromPos = endPos;
                }
                transform.position = fromPos;
            }
        } else {
            rb2d.linearVelocity = Vector2.zero;
        }
    }

    public override void TurnOn()
    {
        isOn = true;
    }

    public override void TurnOff()
    {
        isOn = false;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Vector3 temp = transform.position + (Vector3)goToFromStart;
        Gizmos.DrawSphere(temp, .2f);
        Gizmos.DrawLine(transform.position, temp);
    }
}
