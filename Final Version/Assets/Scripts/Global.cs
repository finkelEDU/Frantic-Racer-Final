using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static float bestTime = 999.999f;

    public void Start()
    {
        Debug.Log(bestTime);
    }
}
