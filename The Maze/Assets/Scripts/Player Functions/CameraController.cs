using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float gap = 3f;
    public float smoothPosSpeed;
    public float smoothRotSpeed;


    float rotX;
    float rotY;
    Quaternion desiredRotation;
    Vector2 framingBalance;

    public float minVerAngle = 0f;
    public float maxVerAngle = 80f;

    private Vector3 currentVelocity = Vector3.zero;
    void Start()
    {
        transform.position = target.position + new Vector3(0,0,gap);
    }


    void LateUpdate()
    {
        if (Input.GetMouseButton((int)MouseButton.Right) && !GetComponent<CameraShake>().enabled){
            rotX += Input.GetAxis("Mouse Y") * smoothRotSpeed;
            rotX = Mathf.Clamp(rotX, minVerAngle, maxVerAngle);
            rotY += Input.GetAxis("Mouse X") * smoothRotSpeed;
            
            desiredRotation = Quaternion.Euler(rotX, rotY, 0);
            var focusPos = target.position + new Vector3(framingBalance.x, framingBalance.y);
            
            transform.position = focusPos - desiredRotation * new Vector3(0, 0, gap); 
            transform.rotation = desiredRotation;
        }
    
        Vector3 desiredPosition = target.position - desiredRotation * new Vector3(0, 0, gap);  
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothPosSpeed);
    }
}

