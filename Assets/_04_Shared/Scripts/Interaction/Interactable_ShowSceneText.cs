using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_ShowSceneText : Interactable
{
    public TextMesh text;
    GalleryMenuItem item;
    public bool reposition = false;
    public bool relative;
    public Vector3 scale;
    public Vector3 position;
    Vector3 initScale;
    Vector3 initPosition;

    private void OnEnable()
    {
        SetupText();

    }

    void SetupText()
    {
        if (item == null)
        {
            if (GetComponentInParent<GalleryMenuItem>() != null)
            {
                item = this.transform.parent.GetComponent<GalleryMenuItem>();
                string t = AddLineBreaks(20);
                t = t.Replace(";", "\n");
                text.text = t;
            }
        }
        else
        {
            string t = AddLineBreaks(20);
            t = t.Replace(";", "\n");

            text.text = t;
        }
    }
    public override void HandleEnter()
    {
        SetupText();
        base.HandleEnter();
        text.gameObject.SetActive(true);
        if (reposition)
        {
            initPosition = text.transform.localPosition;
            initScale = text.transform.localScale;
            text.transform.position = relative ? this.transform.position + position : position;
            text.transform.localScale = scale; //relative ? this.transform.localScale + scale : scale;
        }
    }

    string AddLineBreaks(int max)
    {
        string s = "";
        int c = 0;
        for (int i = 0; i < item.info.description.Length; i++)
        {
            s += item.info.description[i];
            c++;

            if(item.info.description[i].ToString() == ((";").ToString()))
            {
                c = 0;
            }
            if (c > max && item.info.description[i].ToString() == ((" ").ToString()))
            {
                s += "\n";
                c = 0;
                //print(s);
            }
        }
        return s;
    }

    public override void HandleExit()
    {
        base.HandleExit();
        text.gameObject.SetActive(false);
        if (reposition)
        {
            text.transform.localPosition = initPosition;
            text.transform.localScale = initScale;
        }
    }

}
