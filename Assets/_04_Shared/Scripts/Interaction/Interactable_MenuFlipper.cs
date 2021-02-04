using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_MenuFlipper : Interactable
{
    //explodes an object when clicked

    public Menu_Shift menu;
    public bool left;
    public int whichMenuPage = -1;

    public override void HandleHover()
    {
        if(clicked>.5f){
            HandleTrigger();
        }
    }

	public override void HandleTrigger()
	{
        if (whichMenuPage >= 0)
        {
            menu.which = whichMenuPage;
            menu.setMenu = true;
        }
        else
        {
            if (left)
                menu.flipLeft = true;
            else
                menu.flipRight = true;
        }
	}
}
