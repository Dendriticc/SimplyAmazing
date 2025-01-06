using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Camera cam;

    [Range(0, 1f)] float shakeAmount;

    Vector3 originalPos;

    void Awake()
    {
        cam = Camera.main;
        shakeAmount = 0;
        enabled = false;
    }

	void Update()
	{
        originalPos = cam.transform.localPosition;
	    cam.transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
        shakeAmount -= Time.deltaTime;
        if (shakeAmount <= 0) enabled = false;
	}

    public void ShakeCamera(float power)
    {
        enabled = true;
        shakeAmount = power;
    }
}
