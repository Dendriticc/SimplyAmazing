using UnityEngine;
using UnityEngine.AI;

public class EnemyRoam : EnemyBaseState
{
    NavMeshAgent agent;
    LineOfSight sight;
    float roamRange = 10f;
    float safetyPeriod = 5f;
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Roaming");
        agent = enemy.agent;
        sight = enemy.sight;
        agent.speed = 3;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        safetyPeriod -= Time.deltaTime;
        if (sight.CheckForEntity("Player") != Vector3.zero)
        {
            switch (sight.alertStage)
            {
                case LineOfSight.AlertStage.Normal:
                    break;
                case LineOfSight.AlertStage.Alerted:
                    enemy.SwitchState(enemy.chase);
                    break;
                case LineOfSight.AlertStage.Intrigued:
                    enemy.SwitchState(enemy.search);
                    break;
            }
        }
        
        if (agent.remainingDistance <= 0.5f || sight.CheckForEnemy() && safetyPeriod <= 0f) //done with path or close to another enemy
        {
            Vector3 point;
            if (RandomPoint(enemy.transform.position, roamRange, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                agent.destination = point;
                safetyPeriod = 5f;
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

}
