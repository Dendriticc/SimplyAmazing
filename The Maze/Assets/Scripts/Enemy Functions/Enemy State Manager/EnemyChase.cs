using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : EnemyBaseState
{
    NavMeshAgent agent;
    LineOfSight sight;

    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Chasing");
        //Add a sound to indicate transition
        agent = enemy.agent;
        sight = enemy.sight;
        agent.speed = 6f;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        switch (sight.alertStage)
        {
            case LineOfSight.AlertStage.Normal:
                enemy.SwitchState(enemy.roam);
                break;
            case LineOfSight.AlertStage.Alerted:
                agent.SetDestination(enemy.player.transform.position);
                break;
            case LineOfSight.AlertStage.Intrigued:
                enemy.SwitchState(enemy.search);
                break;
        }
    }
}
