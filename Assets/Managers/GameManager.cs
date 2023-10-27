using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public UnityAction<Vector3> OnNoise;

    new void Awake()
    {
        base.Awake();
        OnNoise += (Vector3 pos) => Debug.Log($"Something made noise at {pos}");
    }
}
