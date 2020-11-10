using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 rotation = new Vector3 (-1f, 1f, -1f) * 20f * Time.deltaTime;
        transform.Rotate(rotation);
    }

}
