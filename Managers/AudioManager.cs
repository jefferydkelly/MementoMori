using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager
{
    static AudioManager instance;
    AudioSource fxSource;
    AudioSource bgmSource;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AudioManager();
            }

            return instance;
        }
    }

    private AudioManager()
    {
        GameObject go = new GameObject("Audio Manager");
        fxSource = go.AddComponent<AudioSource>();
        bgmSource = go.AddComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        fxSource.PlayOneShot(clip);
    }

    public void PlayClip(string listID, string entryID)
    {
        AudioClip slater = DatabaseManager.Instance.GetClipFromDatabase("Sounds", "FX", listID, entryID);
        fxSource.PlayOneShot(slater);
    }
}
