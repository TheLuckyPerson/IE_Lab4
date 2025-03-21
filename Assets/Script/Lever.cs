using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public List<Buttonable> doors;

    public bool isOn;

    private Animator animator;
    private AudioSource leverSound;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        leverSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            isOn = !isOn;
            TriggerResponse();
        }
    }

    public virtual void TriggerResponse()
    {
        animator.SetTrigger("trigger");
        leverSound.Play();
        foreach (Buttonable g in doors)
        {
            if (isOn)
            {
                g.TriggerTurnOn();
            }
            else
            {
                g.TriggerTurnOff();
            }
        }
    }
}
