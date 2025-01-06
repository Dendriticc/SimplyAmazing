using System;
using Unity.VisualScripting;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public Material VisionConeMaterial;
    public float VisionRange;
    public float VisionAngle;
    public LayerMask VisionObstructingLayer;
    public int VisionConeResolution = 120;
    Mesh VisionConeMesh;
    MeshFilter MeshFilter_;


    public enum AlertStage
    {
        Normal,
        Intrigued,
        Alerted
    }
    public AlertStage alertStage;
    [SerializeField]
    [Range(0, 100)] public float alertLevel, calmingPeriod;
    float timePassed;

    void Awake()
    {
        alertStage = AlertStage.Normal;
        alertLevel = 0;
        timePassed = 0;
    }

    void Start()
    {
        transform.AddComponent<MeshRenderer>().material = VisionConeMaterial;
        MeshFilter_ = transform.AddComponent<MeshFilter>();
        VisionConeMesh = new Mesh();
        VisionAngle *= Mathf.Deg2Rad;
    }


    void Update()
    {
        DrawVisionCone();
        CheckForEntity("Player");
        timePassed += Time.deltaTime;
    }

    void DrawVisionCone()
    {
        int[] triangles = new int[(VisionConeResolution - 1) * 3];
        Vector3[] Vertices = new Vector3[VisionConeResolution + 1];
        Vertices[0] = Vector3.zero;
        float Currentangle = -VisionAngle / 2;
        float angleIcrement = VisionAngle / (VisionConeResolution - 1);
        float Sine;
        float Cosine;

        for (int i = 0; i < VisionConeResolution; i++)
        {
            Sine = Mathf.Sin(Currentangle);
            Cosine = Mathf.Cos(Currentangle);
            Vector3 RaycastDirection = (transform.forward * Cosine) + (transform.right * Sine);
            Vector3 VertForward = (Vector3.forward * Cosine) + (Vector3.right * Sine);
            if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit hit, VisionRange, VisionObstructingLayer))
            {
                Vertices[i + 1] = VertForward * hit.distance;
            }
            else
            {
                Vertices[i + 1] = VertForward * VisionRange;
            }


            Currentangle += angleIcrement;
        }
        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }
        VisionConeMesh.Clear();
        VisionConeMesh.vertices = Vertices;
        VisionConeMesh.triangles = triangles;
        MeshFilter_.mesh = VisionConeMesh;
    }

    private void UpdateAlertState(bool playerInFOV, Vector3 playerPosition)
    {
        if (playerInFOV) timePassed = 0;
        switch (alertStage)
        {
            case AlertStage.Normal:
                if (playerInFOV)
                    alertStage = AlertStage.Intrigued;
                break;
            case AlertStage.Intrigued:
                if (playerInFOV)
                {
                    alertLevel += Time.deltaTime * 60;
                    if (alertLevel >= 100)
                        alertStage = AlertStage.Alerted;
                }
                else
                {
                    if(timePassed > calmingPeriod) alertLevel -=  Time.deltaTime * 20;
                    if (alertLevel <= 0)
                        alertStage = AlertStage.Normal;
                }
                break;
            case AlertStage.Alerted:
                if (!playerInFOV)
                    alertStage = AlertStage.Intrigued;
                    alertLevel--;
                break;
        }
    }

    public Vector3 CheckForEntity(string objectTag)
    {
        bool playerInFOV = false;
        Collider[] targetsInFOV = Physics.OverlapSphere(
            transform.position, VisionRange);
        Vector3 destination = Vector3.zero;
        foreach (Collider c in targetsInFOV)
        {
            if (c.CompareTag(objectTag))
            {
                Vector3 entityLocation = c.transform.position;
                float signedAngle = Vector3.Angle(
                transform.forward,
                entityLocation - transform.position);
                if (Mathf.Abs(signedAngle) < VisionAngle * Mathf.Rad2Deg / 2 && !Physics.Linecast(transform.position, entityLocation, VisionObstructingLayer))
                {
                    destination = entityLocation;
                    if (objectTag.Equals("Player")) playerInFOV = true;
                }
                break;
            }
        }
        UpdateAlertState(playerInFOV, destination);
        return destination;
    }

    public bool CheckForEnemy()
    {
        Collider[] targetsInFOV = Physics.OverlapSphere(
            transform.position, VisionRange / 5);
        Vector3 destination = Vector3.zero;
        foreach (Collider c in targetsInFOV)
        {
            if (c.CompareTag("Enemy"))
            {
                destination = c.transform.position;
                float signedAngle = Vector3.Angle(
                transform.forward,
                destination - transform.position);
                return true;
            }
        }
        return false;
    }
}

