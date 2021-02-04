using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInfo : MonoBehaviour
{
    public float time;
    public List<bool> scenes;
    public List<bool> initScenes;
    public List<string> sceneNames;
    public List<int> activeScenes;
    public float counter;
    public int whichScene;
    int maxScenes;
    public int maxActiveScenes = 1000;
    public bool useVO;
    //public bool useJukebox;
    //public bool useSingleAudioTrack;
    public float ambientVolume;
    public float mainVolume;
    public bool gameMode = true;
    public bool ping;

    void Start()
    {
        activeScenes = new List<int>();
        if (scenes.Count < 1)
        {
            scenes = new List<bool>();
            for (int i = 0; i < 10; i++)
            {
                scenes.Add(false);
            }
        }
        else
        {
            initScenes = new List<bool>();
            for (int i = 0; i < scenes.Count; i++)
            {
                initScenes.Add(scenes[i]);
                if (scenes[i])
                    activeScenes.Add(i);
            }
        }
        maxScenes = SceneManager.sceneCountInBuildSettings - 1;
        maxScenes = maxScenes > GetSceneCount() ? GetSceneCount() : maxScenes;
    }

    public int GetScene(int which)
    {
        int c = 0;
        int w = 0;
        for (int i = 0; i < scenes.Count; i++)
        {
            if (scenes[i])
            {
                if(c==which)
                    w = i;
                c++;
            }
        }
        return w;
    }

    public int GetNextScene()
    {
        //print(whichScene + " , " + maxScenes);
        //if (whichScene + 1 > GetSceneCount())
        //{
        //    whichScene = 1;
        //}
        //else
        //{
            //bool lastOne = false; ;

        //for (int i = whichScene + 1; i < scenes.Count; i++)
        //{   
        //    if (i == scenes.Count - 1)
        //    {
        //        whichScene = 1;
        //    }
        //    else if (scenes[i])
        //    {
        //        whichScene = i;
        //        break;
        //    }
        //}

        bool found = false;
        for (int i = 0; i < activeScenes.Count; i++)
        {
            if(whichScene == activeScenes[i])
            {
                found = true;
                whichScene = ((i + 1) < activeScenes.Count) ? activeScenes[i+1] : 1;
                break;
            }
        }
        if (!found)
            whichScene = whichScene==activeScenes[0] ? 1 : activeScenes[0];
        //}

        return whichScene;
    }

    public int GetSceneCount()
    {
        int o = 0;
        for (int i = 0 ; i < scenes.Count; i++)
        {
            if (scenes[i])
            {
                o++;
            }
        }
        return o;
    }



    //public bool SceneExists(string s)
    //{
    //    bool o = false;
    //    for (int i = 0; i < scenes.Count; i++)
    //    {
    //        if (scenes[i])
    //        {
    //            o++;
    //        }
    //    }
    //    return o;
    //}

 
}
