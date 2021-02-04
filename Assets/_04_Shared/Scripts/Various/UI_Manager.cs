using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : Interactable
{
    public Renderer ren;
    public float fadeSpeed;
    public SceneInfo sceneInfo;
    int sceneCount;

    public float counter;
    bool counting = false;

    public PlayRandomAudio VOAudio;
    public UnityEngine.Audio.AudioMixer masterMixer;

    //public ButtonIndicator VOIntroIndicator;
    public WithWithoutVOTag[] VOIntroIndicators;

    //ButtonIndicator[] indicators;
    public GameObject game;
    public string WithWithoutVO = "WithWithoutVO";
    bool fading;
    Vector2 volume;

    void Start()
    {
        volume = new Vector2(sceneInfo.mainVolume, sceneInfo.ambientVolume);
        DontDestroyOnLoad(this.gameObject);
        Fade(1);
        StartCoroutine(Loaded());
        sceneCount = sceneInfo.GetSceneCount();
        FindVOButton();
        PlayerPrefs.SetInt("first", 0);
        //indicators = Resources.FindObjectsOfTypeAll(typeof(ButtonIndicator)) as ButtonIndicator[];

        //VOIntroIndicators = new List<WithWithoutVOTag>();
    }

    void FindVOButton()
    {
        //if (VOIntroIndicators == null)
        //{
            //VOIntroIndicators = new List<WithWithoutVOTag>();
        VOIntroIndicators = Resources.FindObjectsOfTypeAll<WithWithoutVOTag>() as WithWithoutVOTag[];
        //}
        //indicators = Resources.FindObjectsOfTypeAll(typeof(ButtonIndicator)) as ButtonIndicator[];
        //for (int i = 0; i < indicators.Length; i++)
        //{
        //    print("indicators: " + indicators[i].gameObject.name);
        //    if (indicators[i].gameObject.name == WithWithoutVO)
        //        VOIntroIndicators.Add(indicators[i].GetComponent<ButtonIndicator>());
        //}
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (counting)
        {
            counter += (Time.deltaTime/60);
            sceneCount = sceneInfo.GetSceneCount();
            if (counter > sceneInfo.time / (float)(sceneCount))// || Input.GetButtonDown("Fire1"))
            {
                int s = sceneInfo.GetNextScene();
                SwitchScene(s);
            }
        }

        //if (indicators.Length == 0)
        //{
        //    indicators = Resources.FindObjectsOfTypeAll(typeof(ButtonIndicator)) as ButtonIndicator[];

        //    //for (int i = 0; i < indicators.Length; i++)
        //    //{
        //    //    //print("indicators: " + indicators[i].gameObject.name);
        //    //    if (indicators[i].gameObject.name == WithWithoutVO)
        //    //        VOIntroIndicators.Add(indicators[i].GetComponent<ButtonIndicator>());
        //    //}
        //}
        //volume = new Vector2(
        //   Mathf.Lerp(volume.x, sceneInfo.mainVolume, Time.deltaTime),
        //   Mathf.Lerp(volume.y, sceneInfo.ambientVolume, Time.deltaTime)
        //   );
        //if (sceneInfo.useVO && sceneInfo.sceneNames[sceneInfo.whichScene] != "MenuWithUI" &&
        //    sceneInfo.useVO && sceneInfo.sceneNames[sceneInfo.whichScene] != "MenuMaster" &&
        //    sceneInfo.sceneNames[sceneInfo.whichScene] != "Loading" &&
        //    !fading)
        //{
           
        //    SetMixerVolume(volume.x, volume.y);
        //}

    }

    public void SwitchScene(int which)
    {
        print("Which Scene: " + which);
        if (which < 0)
        {
            which = sceneInfo.GetNextScene();
        }
        counter = 0;
        if (which == 1)
        {
            print("scene is one");

            VOAudio.StopAndReset();
            VOAudio.skipIntro = false;

            counting = false;
            sceneInfo.scenes = sceneInfo.initScenes;
            sceneInfo.useVO = true;
            sceneInfo.whichScene = which;

            SetMixerVolume(1, 1);
            //VOAudio.gameObject.SetActive(false);
        }
        //else
        //{
        //    SetMixerVolume(sceneInfo.mainVolume,sceneInfo.ambientVolume);
        //}

        StartCoroutine(Load(which));
    }

    void SetMixerVolume(float mainVolume, float ambientVolume)
    {
        //print("Volume: " + mainVolume + " , " + ambientVolume);
        if (masterMixer != null)
        {
            masterMixer.SetFloat("MainVolume", mainVolume);
            masterMixer.SetFloat("AmbientVolume", ambientVolume);
        }
    }

    public void TurnOffVO()
    {
        sceneInfo.useVO = false;

    }

    public void TurnOnVOIntroIndicator()
    {
        //if (VOIntroIndicators==null)
        //    FindVOButton();
        //else if (VOIntroIndicators.Count==0)
            FindVOButton();
        foreach (WithWithoutVOTag b in VOIntroIndicators)
        {
            if(b!=null)
                b.GetComponent<ButtonIndicator>().TurnOn();
            print("VOIntroIndicator On");
        }
    }

    public void TurnOffVOIntroIndicator()
    {
        //if (VOIntroIndicators==null)
        //    FindVOButton();
        //else if (VOIntroIndicators.Count == 0)
            FindVOButton();
        foreach (WithWithoutVOTag b in VOIntroIndicators)
            b.GetComponent<ButtonIndicator>().TurnOff();
        print("VOIntroIndicator Off");
    }

    public void TurnOnVO()
    {
        sceneInfo.useVO = true;
        //if (VOIntroIndicator == null)
        //    FindVOButton();
        //if (!VOAudio.skipIntro)
        //    VOIntroIndicator.TurnOn();
        //else
            //VOIntroIndicator.TurnOff();
    }

    public void ToggleVO()
    {
        sceneInfo.useVO = !sceneInfo.useVO;
        //if(sceneInfo.useVO)
        //    VOIntroIndicator.TurnOn();
        //else
            //VOIntroIndicator.TurnOff();
    }

    void Awake()
    {
        //AudioListener.volume = 0;
        ren.sharedMaterial.color = new Color(0, 0, 0, 1);

    }

    public void Fade(float a)
    {

        if (ren == null)
            ren = GetComponent<Renderer>();
        //print(ren);
        if (a >= 0)
        {
            ren.enabled = true;
        }
        a = Mathf.Clamp(a, 0, 1);
        //AudioListener.volume = (1 - a) * 1;
        //print("Fading; " + a * sceneInfo.ambientVolume);
        float mult = Mathf.Clamp(sceneInfo.ambientVolume, 0, 1);
        float avol = Mathf.Lerp(-80, 1, (1 - a) * mult) ;
        //print("Value: " + (1-a) + " clamped: " + mult + " lerped: " + avol);
        SetMixerVolume(sceneInfo.mainVolume,  avol );// DLUtility.remap( 1-a,0,1,-80,sceneInfo.ambientVolume));
        ren.sharedMaterial.color = new Color(0, 0, 0, a);
        if (a <= 0)
        {
            ren.enabled = false;
        }
    }

    public override void HandleTrigger()
    {
        base.HandleTrigger();
        sceneInfo.GetNextScene();
        //StartCoroutine(Load(sceneInfo.whichScene));
        SwitchScene(sceneInfo.whichScene);

        counting = true;

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Fade(1);
        StartCoroutine(Loaded());
        if (sceneInfo.useVO && 
            sceneInfo.sceneNames[sceneInfo.whichScene] != "MenuWithUI" &&
            sceneInfo.sceneNames[sceneInfo.whichScene] != "MenuMaster" &&
            sceneInfo.sceneNames[sceneInfo.whichScene] != "Loading")
        {
            if (!VOAudio.active)//!VOAudio.audi.isPlaying)
            {
                VOAudio.StopAndReset();
                VOAudio.Play();// gameObject.SetActive(true);
            }
        }
        else
        {
            AudioListener.volume = 1;
            VOAudio.StopAndReset(); //gameObject.SetActive(false);
        }

    }

    IEnumerator Load(int whichScene)
    {
        game.GetComponentInChildren<RingGame>().Reset();
        game.SetActive(false);
        if (whichScene == 0)
        {
            counting = false;
            counter = 0;
        }
        float count = 0;
        fading = true;
        //volume = new Vector2(sceneInfo.mainVolume, sceneInfo.ambientVolume);
        while (count < fadeSpeed)
        {
            count += Time.deltaTime;
            Fade(count / fadeSpeed);
            yield return null;
        }
        fading = false;
        //bool isSceneActive = false;
        //for (int i = 0; i < sceneInfo.activeScenes.Count; i++)
        //{
        //    if (whichScene == sceneInfo.activeScenes[i])
        //        isSceneActive = true;
        //}
        //if (!isSceneActive)
        //    whichScene = sceneInfo.activeScenes[0];
        //print(isSceneActive);
        SceneManager.LoadScene(sceneInfo.sceneNames[whichScene]);
        sceneInfo.whichScene = whichScene;
    }

    IEnumerator Loaded()
    {
        float count = 0;
       
        Fade(1);
        count = 0;
        while (count < fadeSpeed)
        {
            count += Time.deltaTime;
            Fade(1 - (count / fadeSpeed));
            yield return null;
        }
        Fade(0);
        if (sceneInfo.sceneNames[sceneInfo.whichScene] != "MenuWithUI" &&
            sceneInfo.sceneNames[sceneInfo.whichScene] != "MenuMaster" &&
            sceneInfo.sceneNames[sceneInfo.whichScene] != "Loading" && sceneInfo.gameMode)
        {
            game.SetActive(true);
        }
        else
        {
            game.SetActive(false);
        }
    }
}

