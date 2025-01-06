using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    public NavMeshAgent agent;
    public LineOfSight sight;
    public Vector3 LatestTrapTriggerLocation;
    public Cell[,] maze;

    public Player player;
    EnemyBaseState currentState;
    
    public EnemyRoam roam = new EnemyRoam();
    public EnemyChase chase = new EnemyChase();
    public EnemySearch search = new EnemySearch();
    public EnemyInvestigate investigate = new EnemyInvestigate();

    void Start()
    {
        player = FindObjectOfType<Player>();
        maze = FindObjectOfType<MazeGenerator>().GetCells();
        currentState = roam;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Good Item"))
        {
            ScaleAttribute(1.5f);
            // Apply reward
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Bad Item"))
        {
            ScaleAttribute(0.75f);
            // Apply punishment
            Destroy(other.gameObject);
        }
    }

    void ScaleAttribute(float scaleFactor)
    {
        if (Random.Range(0, 1f) > 0.5f) agent.speed *= scaleFactor;
        else sight.VisionRange *= scaleFactor;
    }

    public void SetTriggerLocation(Vector3 location)
    {
        LatestTrapTriggerLocation = location;
        SwitchState(investigate);
    }
}
