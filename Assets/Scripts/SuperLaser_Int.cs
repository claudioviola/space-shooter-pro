using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperLaser_Int : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnAnimationEnd(){
        print("OnAnimationEnd");
        print("parent name:"+transform.parent.name);
        SuperLaser sL = transform.parent.GetComponent<SuperLaser>();
        sL.PlayMe();
    }
}
