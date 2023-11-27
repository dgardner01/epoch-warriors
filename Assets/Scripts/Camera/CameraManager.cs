using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    public float panSpeed;
    public float zoomSpeed;

    Vector3 targetPos;
    Vector3 lerpedPos;

    float targetSize = 5;

    float shakeTimer;
    float shakeTimerMax;
    float shakeMag;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void InitializeTargetPos()
    {
        panSpeed = 0.1f;
        targetPos = (player.transform.position + enemy.transform.position) / 2;
        targetPos.y = 0;
        targetPos.z = -10;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            Shake();
        }
        else
        {
            InitializeTargetPos();
        }
    }

    private void FixedUpdate()
    {
        if (shakeTimer <= 0)
        {
            PanTowardsTarget();
        }
        UpdateCameraSize();
    }

    void Shake()
    {
        panSpeed = 1;
        shakeTimer -= Time.deltaTime;
        float x = Random.Range(-shakeMag, shakeMag);
        float y = Random.Range(-shakeMag, shakeMag);
        transform.position = targetPos;
        transform.position += new Vector3(x, y,0);
    }

    void PanTowardsTarget()
    {
        Vector3 lerpedPos = Vector3.Lerp(transform.position, targetPos, panSpeed);
        transform.position = lerpedPos;
    }

    void UpdateCameraSize()
    {
        float currentSize = Camera.main.orthographicSize;
        Camera.main.orthographicSize = Mathf.Lerp(currentSize, targetSize, zoomSpeed);
    }

    public void Zoom(float orthographicSize)
    {
        targetSize = orthographicSize;
    }

    public IEnumerator Freeze(float freezeTime)
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(freezeTime);
        Time.timeScale = 1;
    }

    public void FreezeThenShake(float freezeTime, float shakeTime, float _shakeMag)
    {
        StartCoroutine(Freeze(freezeTime));
        shakeTimerMax = shakeTime;
        shakeTimer = shakeTimerMax;
        shakeMag = _shakeMag;
    }
}
