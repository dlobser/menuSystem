using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RingGame : MonoBehaviour
{
    public GameObject target;
    public GameObject follower;
    public GameObject ring;
    public GameObject ringContainer;
    public GameObject ringPool;
    public AudioSource audi;
    public float followSpeed = 1;
    public float birthFrequency = 1;
    float bf = 5;
    public float ringSpeed = -1;
    public float ringSpeedMin = -1;
    public float ringSpeedMax = -2;
    public float ringSpread = 5;
    public GameObject boomPool;
    float ringCounter = 0;
    public float ringBarrier = 0;
    public float birthPosition = 10;
    public float ringScale = 1;
    public float killDistance = .5f;
    public GameObject explosion;
    public float boomScale;
    public AimingGame aimingGame;
    float ringTimer = 3;
    public float setAimingTimer;
    int ringScore = 0;
    public string sutra;
    public int whichLetter;
    bool missed = false;
    public Camera gameCam;
    public bool reset;

    void Start()
    {
        follower.transform.localPosition = target.transform.localPosition;
        for (int i = 0; i < 100; i++)
        {
            GameObject g = Instantiate(ring,ringPool.transform);
            g.transform.GetChild(0).GetComponent<LookAtCamera>().lookAtCamera = gameCam;
        }
    }

    public void Reset()
    {
        for (int i = 0; i < boomPool.transform.childCount; i++)
        {
            boomPool.transform.GetChild(0).transform.parent = ringPool.transform;
            print(i + "boompool Reset");
        }
        for (int i = 0; i < ringContainer.transform.childCount; i++)
        {
            print(i + "ringContainer Reset");
            ringContainer.transform.GetChild(0).transform.parent = ringPool.transform;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Reset();
    }
    // Update is called once per frame
    void Update()
    {
        if (reset)
        {
            Reset();
            reset = false;
        }
        ringCounter += Time.deltaTime;
        //ringTimer -= Time.deltaTime;
        //if (ringTimer <= 0)
        //{
        //    aimingGame.hovering = false;

        //}
        //else
        //{

        //    aimingGame.hovering = true;
        //}
        if (missed)
        {
            aimingGame.hovering = false;

            if (ringSpeed < ringSpeedMin)
                ringSpeed += Time.deltaTime * .2f;
        }
        else
        {
            aimingGame.hovering = true;

            if (ringSpeed > ringSpeedMax)
                ringSpeed -= Time.deltaTime * .01f;
        }
        if (bf > birthFrequency)
        {
            //if(ringTimer<=0)
                //bf += Time.deltaTime*.2f;
            //else
                bf -= Time.deltaTime * .05f;
        }
        if (ringCounter > bf)
        {
            ringCounter = 0;
            Transform g = ringPool.transform.GetChild(0);
            g.parent = ringContainer.transform;
            //float off = Mathf.PerlinNoise(Time.time, Time.time)*10;
            float st = Time.time*.1f + Mathf.Sin(Time.time*.2f + Mathf.Sin(Time.time*.06f)*1.5f + Mathf.Sin(Time.time * .133f)*1)*2 + Mathf.Sin(Time.time);
            g.localPosition = new Vector3(Mathf.Cos(st) *ringSpread, Mathf.Sin(st) * ringSpread*.5f, birthPosition);
            g.localScale = Vector3.zero;
            g.GetChild(0).GetComponent<SpriteRenderer>().color = Color.HSVToRGB((Time.time * .1f) % 1, .8f, 1);// Random.ColorHSV(.2f, .5f, .5f, .8f, .8f, 1f);// new Color(1, 1, 1, 1);
            //g.GetChild(1).GetComponent<TextMesh>().text = sutra[whichLetter].ToString();
            //g.GetChild(1).gameObject.SetActive(false);
            //whichLetter += 1;
            //if (whichLetter > sutra.Length - 1)
                //whichLetter = 0;

        }
        for (int i = 0; i < ringContainer.transform.childCount; i++)
        {
            Transform t = ringContainer.transform.GetChild(i);
            t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, t.localPosition.z + ringSpeed * Time.deltaTime);
            if (t.localScale.x < ringScale)
            {
                float s = t.localScale.x;
                s += Time.deltaTime * .25f;
                t.localScale = new Vector3(s, s, s);
            }
            if (t.localPosition.z < ringBarrier)
            {
                t.parent = ringPool.transform;
                if(ringTimer>0)
                    ringTimer -= 1;
            }
            if (t.localPosition.z < target.transform.position.z)
            {
                if(bf<5)
                    bf += Time.deltaTime * .5f;
                missed = true;
            }
            if (Vector3.Distance(t.position, follower.transform.position) < killDistance)
            {
                //audi.Play();
                audi.volume = .2f;
                ringScore += 1;
                audi.pitch = Mathf.Sin(((float)ringScore*Mathf.PI*2)*.1f) * .15f + .7f;
                //Instantiate(explosion, t.transform.position, Quaternion.identity);
                //audi.volume = Random.Range(.1f, .2f);
                t.parent = boomPool.transform;
                ringTimer = setAimingTimer;
                if(ringTimer<2)
                    ringTimer += 1;
                missed = false;
            }
            float dist = Vector3.Distance(t.localPosition, Vector3.zero);
            if (dist < 1)
            {
                Color c = t.GetChild(0).GetComponent<SpriteRenderer>().color;
                t.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, dist);
            }
        }
        for (int i = 0; i < boomPool.transform.childCount; i++)
        {

            Transform t = boomPool.transform.GetChild(i);
            t.Translate(0, 0, ringSpeed * Time.deltaTime);
            float s = t.localScale.x;
            s += Time.deltaTime*5;
            t.localScale = new Vector3(s, s, s);
            Color c = t.GetChild(0).GetComponent<SpriteRenderer>().color*(1+((s-1)*.2f));
            t.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(c.r,c.g,c.b, 1 - ((s - 1.5f) / (boomScale - 1)));
            //t.GetChild(1).gameObject.SetActive(true);
            //t.GetChild(1).GetComponent<TextMesh>().color = new Color(1, 1, 1, 1 - ((s - 1) / (boomScale - 1)));

            if (s > boomScale)
            {
                t.transform.parent = ringPool.transform;
            }
        }
        follower.transform.position = Vector3.Lerp(follower.transform.position, target.transform.position, Time.deltaTime*8);
        audi.volume = audi.volume * .95f;
    }

    //void FixedUpdate()
    //{
    //    follower.transform.position = Vector3.Lerp(follower.transform.position, target.transform.position, .5f);
    //    //Vector3 force = (target.transform.position) - follower.transform.position;
    //    //follower.GetComponent<Rigidbody>().MovePosition( = target.transform.position;// AddForce(force * followSpeed);
    //}
}
