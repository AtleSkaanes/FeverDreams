using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    public bool loop;
    public bool playOnStart;

    public AudioSource source;
}
