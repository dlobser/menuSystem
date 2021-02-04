using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildGalleryMenu : MonoBehaviour
{

    List<GalleryMenuItem> items;
    Texture2D[] textList;
    Sprite[] spriteList;
    string[] files;
    string[] descriptions;
    public string path;
    public bool rebuild;

    public Vector2 spacing;
    [Tooltip("X:X,Y:Y,Z:Pages")]
    public Vector3 howMany;
    //public GameObject button;
    //List<GameObject> buttons;
    public GameObject container;

    public GalleryMenuItem menuItem;

    public bool loaded = false;

    public int[] initialActiveScenes;

    //public Material textMaterial;
    //public Font font;

    private void Start()
    {
        items = new List<GalleryMenuItem>();
        Debug.Log(SceneManager.GetSceneByName("Loading").buildIndex);
    }

    private void Update()
    {
        if (rebuild)
        {
            Rebuild();
            rebuild = false;
        }
    }

    public void Rebuild()
    {
        for (int i = 0; i < container.transform.childCount; i++)
        {
            Destroy(container.transform.GetChild(i).gameObject);
        }
        Populate();
    }

    public void Populate()
    {
        StartCoroutine(LoadImages());
    }

    string CheckForDescription(string file)
    {
        string r = "";
        string f = file.Remove(file.Length - 4);
        for (int i = 0; i < descriptions.Length; i++)
        {
            if (f.Equals(descriptions[i].Remove(descriptions[i].Length - 4)))
            {
                string pathTemp = "file://" + descriptions[i];
                print(pathTemp);

                WWW www = new WWW(pathTemp);
                //r = System.Text.Encoding.UTF8.GetString(www.bytes, 3, www.bytes.Length - 3);

                r = www.text;
                //print(r);
            }
        }
        return r;
    }

    private IEnumerator LoadImages()
    {
        yield return new WaitForSeconds(.1f);

        files = System.IO.Directory.GetFiles(Application.dataPath + path, "*.jpg");
        descriptions = System.IO.Directory.GetFiles(Application.dataPath + path, "*.txt");


        textList = new Texture2D[files.Length];
        spriteList = new Sprite[files.Length];
        string[] descriptionList = new string[files.Length];
        int dummy = 0;


        foreach (string tstring in files)
        {
            string ostring = "";
            string[] pstring = tstring.Split(new string[] { "/" }, System.StringSplitOptions.None);
            ostring = pstring[pstring.Length - 1].Remove(pstring[pstring.Length - 1].Length - 4);
            //Debug.Log(pstring[pstring.Length-1].Remove(pstring[pstring.Length - 1].Length - 4));
            string pathTemp = "file://" + tstring;
            print(ostring);
            WWW www = new WWW(pathTemp);
            yield return www;
            Texture2D texTmp = new Texture2D(1024, 1024, TextureFormat.DXT1, false);
            www.LoadImageIntoTexture(texTmp);
            var sprite = Resources.Load<Sprite>(ostring);
            //Debug.Log((sprite));

            sprite.rect.Set(0,0,sprite.texture.width/1, sprite.texture.height/1);
            spriteList[dummy] = sprite; //Sprite.Create(texTmp, new Rect(0, 0, texTmp.width, texTmp.height), Vector2.one*.5f, texTmp.width);//new Vector2(texTmp.width*.5f,texTmp.height*.5f));
            textList[dummy] = texTmp;

            string d = CheckForDescription(tstring);
            byte[] b = System.Text.Encoding.UTF8.GetBytes(d);
            string dd = System.Text.Encoding.UTF8.GetString(b, 3, b.Length - 3);
            descriptionList[dummy] = dd;
            //Debug.Log(dummy + " " + dd);
            dummy++;


        }

        //int j = 0;
        //int k = 0;
        //int l = 0;

        //GameObject PageParent = new GameObject();
        //PageParent.transform.parent = container.transform;
        //PageParent.name = "PageParent_1";

        //GameObject Page = new GameObject();
        //Page.transform.parent = PageParent.transform;
        //Page.name = "Page_1";


        List<GalleryMenuItem.Info> infos = new List<GalleryMenuItem.Info>();

        Dictionary<string, List<int>> inf = new Dictionary<string, List<int>>();

        for (int i = 0; i < files.Length; i++)
        {
            GalleryMenuItem.Info volume = GalleryMenuItem.Info.CreateFromJSON(descriptionList[i]);
            if (inf.ContainsKey(volume.page))
            {
                inf[volume.page].Add(i);
            }
            else
            {
                inf[volume.page] = new List<int>();
                inf[volume.page].Add(i);

            }
        }
        string[] keys = new string[inf.Keys.Count];
        inf.Keys.CopyTo(keys, 0);
        int l = 0;
        for (int i = 0; i < keys.Length; i++)
        {
            int j = 0;
            int k = 0;


            GameObject PageParent = new GameObject();
            PageParent.transform.parent = container.transform;
            PageParent.name = "PageParent_1";

            GameObject Page = new GameObject();
            Page.transform.parent = PageParent.transform;
            Page.name = "Page_"+i+"_"+keys[i];

            for (int q = 0; q < inf[keys[i]].Count; q++)
            {
                GalleryMenuItem item = Instantiate(menuItem);
                items.Add(item);
                item.transform.localPosition = new Vector3((float)j + .5f, -(float)k - .5f, 0);
                item.transform.Translate(-howMany.x * .5f, howMany.y * .5f, 0);
                item.transform.localScale = new Vector3(spacing.x, spacing.x, spacing.x);
                item.transform.parent = Page.transform;
                item.Build(spriteList[inf[keys[i]][q]], descriptionList[inf[keys[i]][q]]);
                //item.transform.localScale = howMany.x>howMany.y? Vector3.one*(1f/howMany.x) : Vector3.one * (1f / howMany.y);

                //Page.transform.localPosition = new Vector3(0,0, 0);
                j++;
                l++;
                if (j > howMany.x - 1)
                {
                    j = 0;
                    k++;
                }
                if (k > howMany.y - 1)
                {
                    k = 0;
                }

            }

        }

        //for (int i = 0; i < files.Length; i++)
        //{
        //    GalleryMenuItem item = Instantiate(menuItem);
        //    items.Add(item);
        //    item.transform.localPosition = new Vector3((float)j+.5f, -(float)k-.5f, 0);
        //    item.transform.Translate (-howMany.x*.5f, howMany.y*.5f, 0);
        //    item.transform.localScale = new Vector3(spacing.x, spacing.x, spacing.x);
        //    item.transform.parent = Page.transform;
        //    item.Build(spriteList[i], descriptionList[i]);
        //    //item.transform.localScale = howMany.x>howMany.y? Vector3.one*(1f/howMany.x) : Vector3.one * (1f / howMany.y);

        //    //Page.transform.localPosition = new Vector3(0,0, 0);
        //    j++;
        //    l++;
        //    if (j > howMany.x-1)
        //    {
        //        j = 0;
        //        k++;
        //    }
        //    if (k > howMany.y-1)
        //    {
        //        k = 0;
        //    }
        //    if (l >= (howMany.x * howMany.y ) && i!=files.Length-1)
        //    {
        //        Debug.Log(l);
        //        j = 0;
        //        k = 0;
        //        l = 0;

        //        PageParent = new GameObject();
        //        PageParent.transform.parent = container.transform;
        //        PageParent.name = "PageParent_" + (Page.transform.parent.childCount);


        //        Page = new GameObject();
        //        Page.transform.parent = PageParent.transform;
        //        Debug.Log("page");

        //        Page.name = "Page_" + (Page.transform.parent.childCount);
        //    }
        //    //item.sprite = spriteList[j];
        //    //paintings[i].gameObject.isStatic = false;
        //    //float aspect = (float)textList[j].width / (float)textList[j].height;
        //    //float s = 1 / (float)textList[j].width;
        //    //float t = 1 / (float)textList[j].height * s;
        //    //paintings[i].gameObject.isStatic = true;

        //    //if (descriptionList[j].Length > 0)
        //    //{
        //    //    GameObject dd = new GameObject();
        //    //    TextMesh text = dd.AddComponent<TextMesh>();

        //    //    text.fontSize = 50;
        //    //    string tempString = descriptionList[j].Remove(0, 1);
        //    //    text.text = tempString;
        //    //    text.font = font;
        //    //    dd.GetComponent<MeshRenderer>().material = textMaterial;
        //    //    dd.transform.parent = paintings[i].transform;
        //    //    dd.transform.localScale = Vector3.one * .005f;
        //    //    dd.transform.localEulerAngles = Vector3.zero;
        //    //    dd.transform.localPosition = new Vector3(.6f, -.2f, 0);
        //    //}

        //    //j++;
        //    //if (j >= textList.Length)
        //    //{
        //    //    j = 0;
        //    //    if (!repeatPaintings)
        //    //        i = paintings.Length;
        //    //}

        //}

        for (int i = 0; i < container.transform.childCount; i++)
        {
            container.transform.GetChild(i).GetChild(0).transform.localScale = new Vector3(1f / howMany.x, 1f / howMany.x, 1);
            //container.transform.GetChild(i).transform.localPosition = new Vector3(-.5f, (1f/howMany.x), 0);

        }

        GameObject[] buttons = new GameObject[items.Count];
        SceneInfo sceneInfo = FindObjectOfType<SceneInfo>();

        //for (int i = 0; i < items.Count; i++)
        //{
        //    buttons[i] = items[i].GetButton();
        //    //int sceneIndex = items[i].info.sceneIndex;
        //    //items[i].SetScene(sceneIndex);
        //    //sceneInfo.scenes[sceneIndex] = false;
        //    sceneInfo.sceneNames = ExpandArray(sceneInfo.sceneNames, items[i].info.sceneName, items[i].info.sceneIndex);
        //}

        //for (int i = 0; i < items.Count; i++)
        //{
        //    items[i].SetButtons(buttons);
        //}

        //for (int i = 0; i < items.Count; i++)
        //{
        //    if (buttons[i].GetComponent<Interactable_Reticle>().reticle != null)
        //    {
        //        //if (!CheckInt(i))
        //        //{
        //            //buttons[i].GetComponent<Interactable_Reticle>().HandleTrigger();
        //            //buttons[i].GetComponent<Interactable_Reticle>().HandleEnter();
        //            buttons[i].GetComponent<ButtonIndicator>().TurnOff();
        //        //}
        //    }
        //}
        //for (int i = 0; i < sceneInfo.initScenes.Count; i++)
        //{
        //    sceneInfo.initScenes[i] = false;

        //}
        //sceneInfo.activeScenes.Clear();
        //for (int i = 0; i < initialActiveScenes.Length; i++)
        //{
        //    //print(initialActiveScenes[i] + " initial");
        //    buttons[FindButton(buttons, initialActiveScenes[i])].GetComponent<ButtonIndicator>().TurnOn();
        //    sceneInfo.scenes[initialActiveScenes[i]] = true;
        //    sceneInfo.initScenes[initialActiveScenes[i]] = true;
        //    sceneInfo.activeScenes.Add(initialActiveScenes[i]);
        //}

        loaded = true;
    }

    int FindButton(GameObject[] buttons, int j)
    {
        int index = -1;
        for (int i = 0; i < buttons.Length; i++)
        {
            //if (j == buttons[i].GetComponentInParent<GalleryMenuItem>().info.sceneIndex)
                //index = i; //buttons[i].GetComponentInParent<GalleryMenuItem>().info.sceneIndex;
        }
        //print("buttonIndex: " + index);
        return index;
    }
    bool CheckInt(int j)
    {
        bool isin = false;
        for (int i = 0; i < initialActiveScenes.Length; i++)
        {
            if (j == initialActiveScenes[i])
                isin = true;
        }
        return isin;
    }

    List<string> ExpandArray(List<string> array, string value, int index)
    {
        //if (index+1 > array.Length)
        //{
        List<string> temp = new List<string>();//[array.Length > (index+1) ? array.Length : index+1];
        for (int i = 0; i < array.Count; i++)
        {
            temp.Add(array[i]);
        }
        while (temp.Count < index+1)
        {
            temp.Add("");
        }
        //print(value);
        //print(index);
        //print(temp.Count);
        temp[index] = value;
        return temp;
           
        //}
        //else
        //{

        //}
    }

}
