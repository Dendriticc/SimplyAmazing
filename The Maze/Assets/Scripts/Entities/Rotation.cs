using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed;
    Quaternion orientation;
    float time = 0f;

    void Update()
    {
        time += Time.deltaTime;
        
        if (GetComponentInParent<EnemyStateManager>() != null) orientation = Quaternion.Euler(0f, (time * rotationSpeed) % 360, 0f);
        if (GetComponent<Buff>() != null) orientation = Quaternion.Euler(90f, (time * rotationSpeed) % 360, 0f);

        transform.localRotation = orientation;
    }
}
