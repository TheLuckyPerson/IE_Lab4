using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public List<Buttonable> doors;

    public bool isOn;
    private AudioSource buttonSound;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        buttonSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            isOn = true;
            TriggerResponse();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            isOn = false;
            TriggerResponse();
        }
    }

    public virtual void TriggerResponse()
    {
        animator.SetBool("ButtonDown", isOn);
        buttonSound.Play();
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
