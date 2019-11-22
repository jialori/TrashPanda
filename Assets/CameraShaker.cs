using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to the camera, not the cameraHolder
public class CameraShaker : MonoBehaviour
{
    private const float ANGLE_MAX = 4.0f;
    private float seed = 0f;
    private float shakingSpeed = 10.0f;
    public IEnumerator Shake(float duration, float magnitude, Vector3 dir)
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
