using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class EnemyInvestigate : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.agent.destination = enemy.LatestTrapTriggerLocation;
        enemy.agent.speed = 5;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.sight.CheckForEntity("Player") != Vector3.zero)
        {
            enemy.sight.alertLevel = 100;
            enemy.SwitchState(enemy.chase);
        }
        if (enemy.agent.remainingDistance <= 0.5f) enemy.SwitchState(enemy.roam);
    }
}