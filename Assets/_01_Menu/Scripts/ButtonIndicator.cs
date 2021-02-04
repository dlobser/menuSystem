using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonIndicator : Interactable
{
    public GameObject[] active; 
    public GameObject[] inactive;
    public bool on;

    public override void HandleTrigger()
    {
        base.HandleTrigger();
        if (on)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }

    public void TurnOn()
    {
        on = true;
        foreach(GameObject g in active)
        {
            if (g != null)
                g.SetActive(true);
        }
        foreach (GameObject g in inactive)
        {
            if (g != null)
                g.SetActive(false);
        }
    }

    public void TurnOff()
    {
        on = false;
        foreach (GameObject g in active)
        {
            if(g!=null)
                g.SetActive(false);
        }
        foreach (GameObject g in inactive)
        {
            if (g != null)
                g.SetActive(true);
        }
    }

}
