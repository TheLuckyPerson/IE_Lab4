using UnityEngine;

public class Spikes : Buttonable
{
    public bool isOn;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void TurnOn()
    {
        isOn = true;
        anim.SetBool("Toggle", isOn);
    }

    public override void TurnOff()
    {
        isOn = false;
        anim.SetBool("Toggle", isOn);
    }
}
