using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Shift : MonoBehaviour
{
    public Transform containerRotator;
    public Transform containerFront;
    public Transform containerBack;

    public GameObject subMenuContainer;
    public Transform[] subMenus;
    public Transform[] origParents;

    public GameObject Left;
    public GameObject Right;

    public int which;
    public float speed;

    public bool flipRight;
    public bool flipLeft;

    public bool setMenu;

    public bool buildMenu;
    bool loaded = false;

    void Start()
    {

        StartCoroutine(WaitToLoad());

    }

    IEnumerator WaitToLoad()
    {
        if (buildMenu)
        {
            GetComponent<BuildGalleryMenu>().Rebuild();
            while (!GetComponent<BuildGalleryMenu>().loaded)
            {
                yield return null;
            }
            subMenuContainer.SetActive(false);

        }

        if (subMenus == null || subMenus.Length == 0)
        {
         
            subMenus = new Transform[subMenuContainer.transform.childCount];
            for (int i = 0; i < subMenus.Length; i++)
            {
                subMenus[i] = subMenuContainer.transform.GetChild(i);
                subMenuContainer.transform.GetChild(i).GetChild(0).Rotate(0,180,0);
            }
        }

        origParents = new Transform[subMenus.Length];
        for (int i = 0; i < origParents.Length; i++)
        {
            origParents[i] = subMenus[i].transform.parent;
        }

        SetMenu(which);

        loaded = true;
        print("loaded: " + loaded);

    }

    void Update()
    {
        if (loaded)
        {
            if (flipRight)
            {
                StartCoroutine(Flip(false));
                flipRight = false;
            }

            if (flipLeft)
            {
                StartCoroutine(Flip(true));
                flipLeft = false;
            }

            if (setMenu)
            {
                SetMenu(which);
                setMenu = false;
            }
        }
    }

    public void SetMenu(int menu)
    {
        which = menu;

        if (which == subMenus.Length - 1)
        {
            Right.SetActive(false);
        }
        else
        {
            Right.SetActive(true);
        }
        if (which == 0)
        {
            Left.SetActive(false);
        }
        else
        {
            Left.SetActive(true);
        }

        for (int i = 0; i < subMenus.Length; i++)
        {
            subMenus[i].parent = origParents[i];
            ResetTransform(subMenus[i]);
        }

        subMenus[which].parent = containerFront;
        ResetTransform(subMenus[which]);

    }

    IEnumerator Flip(bool left)
    {
        if (!(left && which - 1 < 0) && !(!left && which + 1 > subMenus.Length-1))
        {
            float counter = 0;
            float y = this.transform.localEulerAngles.y;

            subMenus[which].parent = containerFront;
            ResetTransform(subMenus[which]);

            subMenus[left ? which - 1 : which + 1].parent = containerBack;
            ResetTransform(subMenus[left ? which - 1 : which + 1]);

            containerBack.transform.localScale = Vector3.zero;
            while (counter < 1)
            {
                counter += Time.deltaTime / speed;

                float s = Mathf.SmoothStep(0,1, (counter / 1));
                float ss = 1 - s;

                containerBack.transform.localScale = new Vector3(s, s, s);
                containerBack.transform.localPosition = new Vector3(!left ? ss*.5f : ss*-.5f, 0, 0);

                containerFront.transform.localScale = new Vector3(ss, ss, ss);
                containerFront.transform.localPosition = new Vector3(!left ? s * -.5f : s*.5f, 0, 0);
                //containerRotator.transform.localEulerAngles = new Vector3(0, Mathf.SmoothStep(0, 1, counter) * (left ? -180 : 180), 0);
                yield return null;
            }
            containerFront.transform.localScale = Vector3.one;
            containerFront.transform.localPosition = new Vector3(0, 0, 0);
            subMenus[which].parent = origParents[which];
            ResetTransform(subMenus[which]);
            containerRotator.transform.localEulerAngles = Vector3.zero;
            subMenus[left ? which - 1 : which + 1].parent = containerFront;
            ResetTransform(subMenus[left ? which - 1 : which + 1]);

            if (!left)
            {
                which++;
                if (which >= subMenus.Length)
                {
                    which = subMenus.Length - 1;
                }
            }
            else if (left)
            {
                which--;
                if (which <= 0)
                {
                    which = 0;
                }
            }
            if (which == subMenus.Length - 1)
            {
                Right.SetActive(false);
            }
            else
            {
                Right.SetActive(true);
            }
            if (which == 0)
            {
                Left.SetActive(false);
            }
            else
            {
                Left.SetActive(true);
            }
        }

    }

    void ResetTransform(Transform t)
    {
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = Vector3.one;
    }


}
