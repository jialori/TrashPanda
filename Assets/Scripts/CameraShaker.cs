using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// Attached to the camera, not the cameraHolder
public class CameraShaker : MonoBehaviour
{

    float duration = .3f;
    float magnitude = 2f;


    private const float ANGLE_MAX = 4.0f;
    private float seed = 0f;
    private float shakingSpeed = 10.0f;


    public void ShakeCamera()
    {
        transform.DOShakePosition(0.52f, 1.9f, 30, 50, false, true);
    }


    public IEnumerator Shake(Vector3 dir)
    {
        dir = dir.normalized;
        Vector3 orgPos = transform.localPosition;
        Vector3 orgAng = transform.localEulerAngles;
        float timeElasped = 0.0f;

        while (timeElasped < duration)
        {
            //float angX = (Mathf.PerlinNoise(seed+3, Time.time * shakingSpeed) - .5f) * magnitude * ANGLE_MAX;
            //float angY = (Mathf.PerlinNoise(seed+4, Time.time * shakingSpeed) - .5f) * magnitude * ANGLE_MAX;
            float angZ = (Mathf.PerlinNoise(seed+5, Time.time * shakingSpeed) - .5f) * magnitude * ANGLE_MAX;
            Vector3 dd = dir * (Mathf.PerlinNoise(seed, Time.time * shakingSpeed) - .5f) * magnitude * ANGLE_MAX;


            //transform.localEulerAngles = new Vector3(angX, angY, angZ);
            transform.localEulerAngles = new Vector3(dd.x, dd.y, angZ);
            timeElasped += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = orgPos;
    }

}
