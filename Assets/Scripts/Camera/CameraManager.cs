using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    Vector3 targetPos;
    Vector3 lerpedPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void InitializeTargetPos()
    {
        targetPos = (player.transform.position + enemy.transform.position) / 2;
        targetPos.y = 0;
        targetPos.z = -10;
    }

    // Update is called once per frame
    void Update()
    {
        InitializeTargetPos();
    }

    private void FixedUpdate()
    {
        
    }
}
