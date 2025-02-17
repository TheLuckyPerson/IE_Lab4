using UnityEngine;

public class Buttonable : MonoBehaviour
{
    public bool reverseActivation = false;

    public void TriggerTurnOn()
    {
        if (reverseActivation)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }

    public void TriggerTurnOff()
    {
        if (reverseActivation)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }

    public virtual void TurnOn()
    {

    }

    public virtual void TurnOff()
    {

    }
}
