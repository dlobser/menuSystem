using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomAudio : MonoBehaviour
{
    //public AudioClip[] clips;
    public List<AudioClip> clipList;
    public AudioClip introClip;

    public GameObject audioClipContainer;

    public float timeBetweenAudio;
    float timeBetweenCounter;

    public AudioSource audi;
    public int which = 0;

    public float audioLength;
    public float lengthCounter;

    bool waiting = false;

    public string path;
    
    public bool skipIntro = false;
    public bool active;

    public bool introHasBeenPlayed = false;

    public void Skip()
    {
        which = 1;
        skipIntro = true;
        print("skip");
    }

    public void ContainerToClipList()
    {
        introClip = audioClipContainer.transform.GetChild(0).GetComponent<AudioSource>().clip;

        clipList = new List<AudioClip>();
        for (int i = 1; i < audioClipContainer.transform.childCount; i++)
        {
            clipList.Add(audioClipContainer.transform.GetChild(i).GetComponent<AudioSource>().clip);
        }

        if (skipIntro)
        {
            print("skipintro");
            audi.clip = clipList[0];
        }
        else
        {
            print("else");
            audi.clip = introClip;
        }
    }

    public void Play()
    {
        //audi.Stop();
        active = true;
        print("Audio Play");
        waiting = true;
        //lengthCounter = 0;

    }

    public void StopAndReset()
    {
        //print("Audio Stop and Reset");
        if (audi == null)
        {
            audi = GetComponent<AudioSource>();
        }

        audi.Stop();
        audi.volume = 0;
        lengthCounter = 0;

        which = 0;
        active = false;
        //timeBetweenCounter = 0; //timeBetweenAudio;
        timeBetweenCounter = timeBetweenAudio * .9f;
        introHasBeenPlayed = false;
        Shuffle(clipList);
        audioLength = 0;

    }

    void Start()
    {
        audi = GetComponent<AudioSource>();
        timeBetweenCounter = timeBetweenAudio * .9f;
        if (audioClipContainer != null)
        {
            ContainerToClipList();

        }
        //print("start");
        Shuffle(clipList);
    }

    void Stop()
    {
        //print("Stop");
        active = false;
        audi.Stop();
    }

    void Update()
    {
        if (active)
        {
            //print("Audio is active? " + active + " Waiting? " + waiting + " Audio playing: " + audi.isPlaying + " between: " +
            //(timeBetweenCounter > timeBetweenAudio));// + " Time " + timeBetweenCounter);

            //if (introClip == null)
            //{
            //    ContainerToClipList();
            //}
            if (!waiting)
                timeBetweenCounter += Time.deltaTime;

            if (!audi.isPlaying)
            {
                if (!skipIntro && !introHasBeenPlayed)
                {
                    //print("skippy doo");
                    if (introClip != null)
                    {
                        audi.clip = introClip;
                        audioLength = audi.clip.length;
                        //print("intro clip != null");
                    }
                    audioLength = audi.clip.length;
                    if (!audi.isPlaying)
                    {
                        audi.Play();
                        audioLength = audi.clip.length;

                        //print("noobly");
                    }

                }
                //else
                //{

                //    audi.clip = clipList[which];
                //    audioLength = audi.clip.length;
                //    which++;
                //    if (which > clipList.Count - 1)
                //    {
                //        Shuffle(clipList);
                //        which = 0; //skipIntro ? 1 : 0;
                //    }
                //}

            }

            if (timeBetweenCounter > timeBetweenAudio)
            {
                if (!skipIntro && !introHasBeenPlayed)
                {
                    //print("blah blah");
                    if (introClip != null)
                    {
                        audi.clip = introClip;
                        audioLength = audi.clip.length;

                        //print("Skip intro && played");
                    }

                }
                else
                {
                    //if (which == 0)
                    //{
                    //    which = 1;
                    //}
                    //print("audi.clip");
                    audi.clip = clipList[which];
                    audioLength = audi.clip.length;
                    which++;
                    if (which > clipList.Count - 1)
                    {
                        Shuffle(clipList);
                        which = 0; //skipIntro ? 1 : 0;
                        //print("shuffling");
                    }
                }
                timeBetweenCounter = 0;
                waiting = true;
                //if (which == 1)
                //{
                //    this.transform.GetChild(0).gameObject.SetActive(false);
                //}
                //print("Boom");
                audi.Play();
                audioLength = audi.clip.length;

            }
            if (waiting)
            {
                //if (audi.isPlaying && skipIntro && audi.clip == clipList[0])
                //{
                //    lengthCounter = audioLength;
                //    audi.Stop();
                //    print("1");
                //}

                //if (audi.isPlaying && audi.clip != clipList[which - 1] || which==0)
                //{
                //    lengthCounter = audioLength;
                //    audi.Stop();
                //    print("2");
                //}


                if (lengthCounter < audioLength)
                {
                    if (!audi.isPlaying)
                    {

                        if (!skipIntro && !introHasBeenPlayed)
                        {
                            //print("introhasbeenplayed");
                            introHasBeenPlayed = true;
                            audi.clip = introClip;
                            audioLength = audi.clip.length;


                        }
                        else
                        {
                            audi.clip = clipList[which];
                            audioLength = audi.clip.length;

                        }
                        audi.Play();
                        //print("line 205");
                    }
                    lengthCounter += Time.deltaTime;
                    float vol = Mathf.Min(1, ((Mathf.Cos((lengthCounter / audioLength) * 6.28f) * -.5f) + .5f) * audioLength * 30);
                    //print();
                    audi.volume = vol;
                    //print("3");
                    if (!skipIntro && !introHasBeenPlayed)
                    {
                        //print("introhasbeenplayed");
                        introHasBeenPlayed = true;
                    }
                }
                else
                {
                    audi.Stop();
                    waiting = false;
                    lengthCounter = 0;
                    //print("4");
                }
            }
        }
        else if (audi != null)
        {
            if (audi.isPlaying) { 
                audi.Stop();
                //print("not null stop");
            }
            //print("audi not null");
        }
    }

  

    public void Shuffle(List<AudioClip> list)
    {
        //print("shuffle");
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, list.Count);
            AudioClip value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
