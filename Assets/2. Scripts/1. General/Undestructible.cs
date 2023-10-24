using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undestructible : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
