using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_SetSceneData : Interactable
{

    SceneInfo sceneInfo;
    public bool setTime;
    public bool setScene;
    public bool addScene;
    //public int maxActiveScenes = 1000;
    public bool switchScene;
    public bool advanceScene;
    public bool setMaxScenes;
    public int maxScenes;
    public bool setVO;
    public bool VOIsOn;
    public bool toggleVO;
    public bool SetVOIntroVisible;
    public bool VOIntroVisible;
    public float time;
    public List<int> scenes;
    public bool setGame = false;
    public bool gameIsOn = false;

    //public GameObject[] buttons;
    //public bool useButtonIndicator = false;

    public bool setVolume;
    public float mainVolume;
    public float ambientVolume;

    public bool forceButtonsToSceneSettings = true;

    public bool ping;

    UI_Manager sceneUI;// = FindObjectOfType<UI_Manager>();

    //public override void HandleHover()
    //{
    //    if(clicked>.5f){
    //        HandleTrigger();
    //    }
    //}

    //int FindButton(GameObject[] buttons, int j)
    //{
    //    int index = -1;
    //    for (int i = 0; i < buttons.Length; i++)
    //    {
    //        if (j == buttons[i].GetComponentInParent<GalleryMenuItem>().info.sceneIndex)
    //            index = i; //buttons[i].GetComponentInParent<GalleryMenuItem>().info.sceneIndex;
    //    }
    //    print("buttonIndex: " + index);
    //    return index;
    //}


    public override void HandleTrigger()
    {
        base.HandleTrigger();

        if (sceneInfo == null)
        {
            sceneInfo = FindObjectOfType<SceneInfo>();
        }
        if (sceneUI == null)
        {
            sceneUI = FindObjectOfType<UI_Manager>();
        }
        if (toggleVO)
        {
            sceneUI.ToggleVO();
        }
        if (setVO)
        {
            if (sceneUI == null)
            {
                sceneUI = FindObjectOfType<UI_Manager>();
            }
            if (sceneUI != null)
            {
                if (VOIsOn)
                    sceneUI.TurnOnVO();
                else
                    sceneUI.TurnOffVO();
            }
        }
        if (SetVOIntroVisible)
        {
            //if (sceneUI.VOIntroIndicator != null)
            //{
                if (VOIntroVisible)
                    sceneUI.TurnOnVOIntroIndicator();// VOIntroIndicator.TurnOn();
                else
                    sceneUI.TurnOffVOIntroIndicator();//.TurnOff();
            //}
        }
        if (setTime)
        {
            sceneInfo.time = time;
        }


        if (setVolume)
        {
            sceneInfo.mainVolume = mainVolume;
            sceneInfo.ambientVolume = ambientVolume;
        }
        //else
        //{
        //    sceneInfo.mainVolume = 1;
        //    sceneInfo.ambientVolume = 1;
        //}
        if (setMaxScenes)
        {
            sceneInfo.maxActiveScenes = maxScenes;
        }
        if (setScene)
        {
            ManageActiveScenes manager = FindObjectOfType<ManageActiveScenes>();// sceneInfo.GetComponent<ManageActiveScenes>();
            manager.SetActiveScenes(scenes);
            if(GetComponentInParent<ButtonIndicator>()!=null)
                GetComponentInParent<ButtonIndicator>().TurnOn();
            if (forceButtonsToSceneSettings)
                manager.ActivateButtonsBasedOnActiveScenes();
            //sceneInfo.whichScene = scenes[0];
        }

        if (addScene)
        {
            ManageActiveScenes manager = sceneInfo.GetComponent<ManageActiveScenes>();
            for (int i = 0; i < scenes.Count; i++)
            {
                bool flip = manager.AddScene(scenes[i]);
                if (!flip)
                {
                    //print(flip);
                    GetComponentInParent<ButtonIndicator>().TurnOff();
                }
                else
                {
                    GetComponentInParent<ButtonIndicator>().TurnOn();
                }
            }
        }
        if (ping)
        {
            sceneInfo.ping = true;
        }
        if (setGame)
        {
            sceneInfo.gameMode = gameIsOn;
        }
        if (advanceScene)
        {
            sceneUI.SwitchScene(-1);
        }
        if (switchScene)
        {
            sceneUI.SwitchScene(scenes[0]);
        }
    }
}
