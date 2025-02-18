using UnityEngine;

public class Door : Buttonable
{
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void TurnOn()
    {
        animator.SetBool("On", true);
    }

    public override void TurnOff()
    {
        animator.SetBool("On", false);

    }
}
