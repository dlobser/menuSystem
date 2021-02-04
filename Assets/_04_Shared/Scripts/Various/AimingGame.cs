using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingGame : Interactable
{
    public GameObject text;
    public MeshRenderer blindTunnel;
    public bool hovering;
    public float hCounter = 0;
    public float hoverSpeed = .1f;
    public float downMult;
    public float upMult;
    public float textFadeCounter = 0;
    float startCounter = 0;

    public override void HandleHover()
    {
        base.HandleHover();
        hovering = true;
    }
    public override void HandleExit()
    {
        base.HandleExit();
        hovering = false;
    }

    void Start()
    {
        text.GetComponent<TextMesh>().color = new Color(1, 1, 1, 0);

    }

    // Update is called once per frame
    public override void HandleUpdate()
    {
        base.HandleUpdate();
        if (hovering)
        {
            if(hCounter<1)
                hCounter += Time.deltaTime * hoverSpeed * upMult;
            //blindTunnel.enabled = true;
            if (textFadeCounter > 0)
            {
                textFadeCounter -= Time.deltaTime;
            }
        }
        else
        {
            if (hCounter > 0) {
                hCounter -= Time.deltaTime * hoverSpeed * downMult;

            }
            else
            {
                //blindTunnel.enabled = false;
                if (textFadeCounter < 1)
                {
                    textFadeCounter += Time.deltaTime;
                }
            }
        }
        if(startCounter<3)
            startCounter += Time.deltaTime;

        else
            text.GetComponent<TextMesh>().color = new Color(1, 1, 1, textFadeCounter);

        AudioListener.volume = hCounter;
        blindTunnel.sharedMaterial.SetFloat("_Fade", hCounter);
    }
}
