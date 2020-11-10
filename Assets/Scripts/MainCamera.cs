using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MainCamera : MonoBehaviour
{

    private PlayableDirector _director;

    // Start is called before the first frame update
    void Start()
    {
        _director = GetComponent<PlayableDirector>();
    }

    public void ShakeMe(){
        _director.Play();
    }
}
