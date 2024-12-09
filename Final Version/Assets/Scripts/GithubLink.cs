using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GithubLink : MonoBehaviour
{
    public string url;

    public void open()
    {
        Application.OpenURL(url);
    }
}
