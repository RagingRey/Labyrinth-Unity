using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Patrol : MonoBehaviour
    {
        public List<Transform> PatrolPoints = new List<Transform>();
        public List<HoverBotScript> EnemiesToAssign = new List<HoverBotScript>();

        void Start()
        {
            foreach (var enemy in EnemiesToAssign)
            {
                enemy.PatrolPath = this;
            }
        }

        public float GetDistanceFromPositionToAPatrolPoint(Vector3 hoverBotPosition, int patrolPointIndex)
        {
            if(patrolPointIndex < 0 || patrolPointIndex >= PatrolPoints.Count || PatrolPoints[patrolPointIndex] == null)
            {
                return -1f;
            }

            return (PatrolPoints[patrolPointIndex].position - hoverBotPosition).magnitude;
        }

        public Vector3 GetPositionOfPatrolPoint(int pointIndex)
        {
            if (pointIndex < 0 || pointIndex >= PatrolPoints.Count || PatrolPoints[pointIndex] == null)
            {
                return Vector3.zero;
            }

            return PatrolPoints[pointIndex].position;
        }
    }
}
