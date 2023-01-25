using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    [SerializeField]
    private float _cameraShakeTime = 1f;
    [SerializeField]
    private AnimationCurve _cameraShakeCurve;

    private bool _isCameraShaking;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CameraShakeRoutine()
    {
        _isCameraShaking = true;

        Vector3 intialPosition = transform.position;

        float runTime = 0;

        while(runTime < _cameraShakeTime)
        {
            runTime += Time.deltaTime;
            float shakeMagnitude = _cameraShakeCurve.Evaluate(runTime / _cameraShakeTime);
            transform.position = intialPosition + Random.insideUnitSphere * shakeMagnitude;
            yield return null;
        }

        transform.position = intialPosition;
        _isCameraShaking = false;
    }

    public void ShakeCamera()
    {
        StartCoroutine(CameraShakeRoutine());

        if(_isCameraShaking)
        {
            _isCameraShaking = false;
            StopCoroutine(CameraShakeRoutine());
        }
    }
}
