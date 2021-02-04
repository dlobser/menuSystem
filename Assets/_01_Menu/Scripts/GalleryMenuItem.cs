using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GalleryMenuItem : MonoBehaviour
{
    public string description;
    public Sprite sprite;
    SpriteRenderer spriteRenderer;
    public GameObject button;
    public Vector3 buttonPosition;
    GameObject _button;

    public TextAsset text;

    [System.Serializable]
    public class Info
    {
        public string page;
        public string sceneName;
        public string description;
        //public int sceneIndex;
        public int[] sceneIndeces;
        public static Info CreateFromJSON(string jsonString)
        {
            print(jsonString);
            Info info = JsonUtility.FromJson<Info>(jsonString);
            print(info);
            return info;
        }
    }

    public Info info;
    public bool write;


    //public string Page { get { return info.page; } set { info.page = value; } }
    //public string SceneName { get { return info.sceneName; } set { info.page = sceneName; } }

    private void Update()
    {
        if (write)
        {
            WriteJSON();
            write = false;
        }
    }
    public void WriteJSON()
    {
        string t = JsonUtility.ToJson(info);
        Debug.Log(t);
        #if UNITY_EDITOR
        File.WriteAllText(AssetDatabase.GetAssetPath(text), t);
        EditorUtility.SetDirty(text);
        #endif
    }

    public void SetButtons(GameObject[] buttons)
    {
        print("gallery menu set buttons");
        //GetComponentInChildren<Interactable_SetSceneData>().buttons = buttons;
    }

    public GameObject GetButton()
    {
        return _button;
    }

    public void SetScene(int w)
    {
        print("gallery menu set scene");
        //GetComponentInChildren<Interactable_SetSceneData>().scene = w;
    }

    public void SetScenes(int[] w)
    {
        print("gallery menu set scenes");
        //GetComponentInChildren<Interactable_SetSceneData>().scene = w[0];
        //if (w.Length > 1)
        //{
        //    for (int i = 1; i < w.Length; i++)
        //    {

        //    }
        //}
    }

    public void Build(Sprite sp, string text)
    {
        sprite = sp;
        description = text;
        spriteRenderer = this.gameObject.AddComponent<SpriteRenderer>();
       
        spriteRenderer.sprite = sprite;
        //Debug.Log(text);
        //Debug.Log(sp.name);
        info = Info.CreateFromJSON(text);
       
        _button = Instantiate(button);
        _button.transform.parent = this.transform;
        _button.transform.localPosition = buttonPosition;

        Interactable_SetSceneData setData = GetComponentInChildren<Interactable_SetSceneData>();
        setData.scenes = info.sceneIndeces.ToList<int>();
    }
}
