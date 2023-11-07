using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public GameObject[] targets;
    public float yOffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 center = Vector3.zero;
        foreach(GameObject target in targets)
        {
            center += target.transform.position;
        }
        center /= targets.Length;
        center.y = yOffset;
        center.z = -10;
        transform.position = Vector3.Lerp(transform.position,center,0.1f);
    }
}
