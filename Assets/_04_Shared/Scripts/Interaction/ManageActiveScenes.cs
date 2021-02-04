using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageActiveScenes : MonoBehaviour
{
    //looks for gameobject named 'root'
    //GameObject container;
    public GalleryMenuItem[] menuItems;
    public Dictionary<int, List<GalleryMenuItem>> dictionary;
    SceneInfo sceneInfo;
    bool initialized = false;

    void Start()
    {

    }

    public void SetActiveScenes(List<int> scenes)
    {

        SetMaxActiveScenes(scenes.Count);
        sceneInfo.activeScenes.Clear();
        for (int i = 0; i < scenes.Count; i++)
        {
            sceneInfo.activeScenes.Add(scenes[i]);
        }

        ToggleScenes();
    }

    void ToggleScenes()
    {
        //if (menuItems[0] == null)
        //{
            Init(true);
        //}
        for (int i = 0; i < menuItems.Length; i++)
        {
            if (menuItems[i].GetComponentInChildren<ButtonIndicator>()!=null)
                menuItems[i].GetComponentInChildren<ButtonIndicator>().TurnOff();
        }
        for (int i = 0; i < sceneInfo.scenes.Count; i++)
        {
            sceneInfo.scenes[i] = false;
        }
        //dictionary[sceneInfo.activeScenes[0]][0].gameObject.GetComponentInChildren<ButtonIndicator>().TurnOn();
        foreach (int key in sceneInfo.activeScenes)
        {
            //foreach(GalleryMenuItem g in dictionary[key])
            //{
            //    g.GetComponentInChildren<ButtonIndicator>().TurnOn();
            //}
            print(key);
            sceneInfo.scenes[key] = true;
        }
    }

    public void ActivateButtonsBasedOnActiveScenes()
    {
        foreach (int key in sceneInfo.activeScenes)
        {
            foreach(GalleryMenuItem g in dictionary[key])
            {
                g.GetComponentInChildren<ButtonIndicator>().TurnOn();
            }
        }
    }

    public bool AddScene(int scene)
    {
        bool canAdd = true;
        bool skipAdd = false;
        for (int s = 0; s < sceneInfo.activeScenes.Count; s++)
        {
            if (sceneInfo.activeScenes[s] == scene)
            {
                if (sceneInfo.activeScenes.Count > 1)
                {
                    canAdd = false;
                    sceneInfo.activeScenes.RemoveAt(s);
                    sceneInfo.scenes[scene] = false;
                }
                else
                {
                    canAdd = true;
                    skipAdd = true;
                }
            }
        }
        if (canAdd && !skipAdd)
        {
            sceneInfo.activeScenes.Add(scene);
            List<int> removed = new List<int>();
            while (sceneInfo.activeScenes.Count > sceneInfo.maxActiveScenes)
            {
                sceneInfo.activeScenes.RemoveAt(0);
            }
            ToggleScenes();
            ActivateButtonsBasedOnActiveScenes();
        }
        //print("can add: " + canAdd);
        return canAdd;
    }

    void SetMaxActiveScenes(int max)
    {
        List<int> removed = new List<int>();
        //print(sceneInfo);
        if (sceneInfo == null)
            sceneInfo = GetComponent<SceneInfo>();
        if (sceneInfo.maxActiveScenes > max)
        {
            for (int i = 0; i < sceneInfo.activeScenes.Count; i++)
            {
                removed.Add(sceneInfo.activeScenes[0]);
                sceneInfo.activeScenes.RemoveAt(0);
            }
        }
        sceneInfo.maxActiveScenes = max;
    }

    void Init(bool doItAnyway = false)
    {
        if ((!initialized && GameObject.Find("Root") != null)||doItAnyway)
        {
            dictionary = new Dictionary<int, List<GalleryMenuItem>>();
            //container = GameObject.Find("Root");
            menuItems = Resources.FindObjectsOfTypeAll(typeof(GalleryMenuItem)) as GalleryMenuItem[];

            //build dictionary
            for (int i = 0; i < menuItems.Length; i++)
            {
                //print("item: " + i);
                //print("len: " + menuItems[i].info.sceneIndeces.Length);
                //print("name: " + menuItems[i].gameObject.name + " : " + menuItems[i].transform.parent.name);
                int j = menuItems[i].info.sceneIndeces[0];
                if (!dictionary.ContainsKey(j))
                {
                    dictionary[j] = new List<GalleryMenuItem>();
                }
                dictionary[j].Add(menuItems[i]);
            }

            sceneInfo = GetComponent<SceneInfo>();
            initialized = true;
        }
    }
    void Update()
    {
        Init();
    }
}
