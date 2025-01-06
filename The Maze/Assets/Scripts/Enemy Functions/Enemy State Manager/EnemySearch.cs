using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySearch : EnemyBaseState
{
    LineOfSight sight;
    NavMeshAgent agent;
    Cell[,] maze;
    List<string> directions = new List<string>();
    List<int> VisitedX = new List<int>();           // x-coordinates of all visited cells
    List<int> VisitedY = new List<int>();           // and the y-coordinates
    List<int> StackX = new List<int>();
    List<int> StackY = new List<int>();
    Vector3 LastRecordedPosition;

    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Looking for player");
        sight = enemy.sight;
        agent = enemy.agent;
        agent.speed = 4;
        maze = enemy.maze;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        LastRecordedPosition = enemy.transform.position;

        switch(sight.alertStage)
        {
            case LineOfSight.AlertStage.Normal:
                enemy.SwitchState(enemy.roam);
                break;
            case LineOfSight.AlertStage.Alerted:
                enemy.SwitchState(enemy.chase);
                break;
            case LineOfSight.AlertStage.Intrigued:
                break;
        }

        if (sight.CheckForEntity("Player") != Vector3.zero) agent.SetDestination(sight.CheckForEntity("Player"));
        else if (Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < sight.VisionRange / 2) agent.SetDestination(enemy.player.transform.position);
    }


}
