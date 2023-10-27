using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Resposible for playing global sounds
/// </summary>
public class SoundManager: Singleton<SoundManager>
{
    public Sound[] sounds;

    new void Awake()
    {
        base.Awake();

        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].source = gameObject.AddComponent<AudioSource>();
            sounds[i].source.clip = sounds[i].clip;

            sounds[i].source.volume = sounds[i].volume;
            sounds[i].source.loop = sounds[i].loop;
        }
    }

    void Start()
    {
        foreach (Sound sound in sounds)
            if (sound.playOnStart)
                sound.source.Play();
    }

    public void Play(string name)
    {
        Sound? audio = Array.Find(sounds, sound => sound.name == name);
        if (audio == null)
        {
            Debug.LogWarning($"Couldn't find audioclip of name {name}");
            return;
        }

        audio?.source.Play();
    }
}
