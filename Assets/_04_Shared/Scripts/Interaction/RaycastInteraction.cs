using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ON;

public class RaycastInteraction : MonoBehaviour {
    
    [Tooltip("Used to link specific raycasters with interactables")]
    public string type;

    public Vector3 hitPosition { get; set; }
    public Vector3 hitNormal{ get; set; }
    public GameObject hitObject{ get; set; }

    public bool useMouse;

	public delegate void MouseHasHit();
	public static event MouseHasHit mouseHasHit;
    [Tooltip("turn this off to only raycast when a button is clicked")]
    public bool alwaysActive = true;
    [Tooltip("Drag in a button component, or leave this slot empty")]
    public Button button;
    float click;


	void Start() {
        if (button == null)
            alwaysActive = true;
	}

	void Update() {

        if (alwaysActive || button != null && button.click > .5f)
        {
            RaycastHit hitInfo = new RaycastHit();
            RaycastHit[] hitsInfo = new RaycastHit[0];

            bool hit;

            if (useMouse)
            {
                hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                hitsInfo = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));

            }
            else
            {
                hit = Physics.Raycast(new Ray(this.transform.position, this.transform.forward), out hitInfo, 1e6f);
                hitsInfo = Physics.RaycastAll(new Ray(this.transform.position, this.transform.forward));
            }

            if (hit)
            {
                hitPosition = hitInfo.point;
                hitNormal = hitInfo.normal;
                hitObject = hitInfo.collider.gameObject;

                click = 0;
                if (button != null)
                    click = button.click;

                if (hitsInfo.Length > 1)
                {
                    bool blocked = false;
                    foreach (RaycastHit h in hitsInfo)
                    {

                        if (h.transform.gameObject.GetComponent<Interactable_Blocker>() != null)
                            blocked = true;
                        

                    }
                    if (!blocked)
                    {
                        foreach (RaycastHit h in hitsInfo)
                        {
                            if (h.transform.gameObject.GetComponent<Interactable>() != null)
                            {
                                Interactable[] interactables = h.transform.gameObject.GetComponents<Interactable>();
                                foreach (Interactable i in interactables)
                                {
                                    i.Ping(this, click, type);
                                }
                                if (h.transform.gameObject.GetComponent<Interactable_Blocker>() != null)
                                    break;
                            }

                        }
                    }
                }
                else if (hitInfo.transform.gameObject.GetComponent<Interactable>() != null)
                {
                    Interactable[] interactables = hitInfo.transform.gameObject.GetComponents<Interactable>();
                    foreach (Interactable i in interactables)
                        i.Ping(this, click, type);
                }

            }
            else
            {
                hitPosition = Vector3.zero;
                hitNormal = Vector3.zero;
                hitObject = null;
            }
        }

	}
}

