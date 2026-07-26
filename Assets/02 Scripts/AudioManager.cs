using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
 
    public static AudioManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    //____________________________________________________________________

    public void Play ( string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(s == null) {
            return;
        }

        s.source.Play();
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + "not found");
            return;
        }

        s.source.Stop();
    }

    //_______________________________________________________________________

    public void VolumeDown(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + "not found");
            return;
        }

        s.source.volume = 0f;
        s.source.pitch = 1f;
    }

    public void VolumeUp(string name, float goal)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + "not found");
            return;
        }

        s.source.volume = goal;
    }

    //___________________________________________________________________
    //___________________________________________________________________

 
    public void SlowMotion(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        s.source.pitch = 0.82f;
    }

    public void NormalMotion(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        s.source.pitch = 1f;
    }

    //____________________________________________________________________

    public void FadeIn(string name, float waittime, float startpoint ,float endpoint)
    {
        StartCoroutine(FadeIn(waittime, name, startpoint, endpoint));
    }

    public void FadeOut(string name, float waittime)
    {
        StartCoroutine(FadeOut(waittime, name));
    }


    IEnumerator FadeOut(float waitTime, string name) //es wird leiser
    {
        Debug.Log("Fadeout");
        Sound s = Array.Find(sounds, sound => sound.name == name);

        float volume = s.source.volume;

        for (float i = volume; i > 0; i -= 0.05f)
        {
            s.source.volume -= 0.05f;
            yield return new WaitForSeconds(waitTime);
        }

        Stop(name);

        yield return 0;
    }

    IEnumerator FadeIn(float waitTime, string name, float startpoint, float endpoint) //es wird lauter
    {
        //Debug.Log("Fadein");
        Sound s = Array.Find(sounds, sound => sound.name == name);

        float volume = s.source.volume;

        for (float i = startpoint; i < endpoint; i += 0.05f)
        {
            s.source.volume += 0.05f;
            yield return new WaitForSeconds(waitTime);
        }
        yield return 0;
    }

    //________________________________________________________________________

    public void LowVolume(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        s.source.volume = 0.1f;

    }

    public void StopAllSounds()
    {

        for(int i = 3; i < sounds.Length; i++)
        {
            Sound s = sounds[i];

            s.source.Stop();
        }
    }
}
