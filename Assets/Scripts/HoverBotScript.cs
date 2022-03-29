using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public enum RobotState
    {
        Patrol,
        Follow,
        Attack
    }

    public class HoverBotScript : MonoBehaviour
    {
        [HideInInspector] public GameObject thisGameObject;
        GameObject m_Player;
        [HideInInspector] public NavMeshAgent Agent;

        public const float detectionMagnitude = 20f;
        public Transform WeaponRoot;
        public Transform detectedPlayerPosition;

        [Header("Shooting")] public GameObject bullet;
        public Transform rootPosition;
        public Transform rootRotation;

        private bool m_ThreatDetected;
        float nextUpdate = 3f;
        private int m_PatrolPointIndexToMoveTo;
        private int PatrolReachingRadius = 2;

        public AudioClip shoot;
        AudioSource _audio;

        private Manager _manager;
        [HideInInspector] public Patrol PatrolPath;
        public LayerMask Layer;

        List<Collider> playerCollider = new List<Collider>();
        public RobotState State;

        void Start()
        {
            State = RobotState.Patrol;
            m_Player = GameObject.FindWithTag("Player");
            thisGameObject = gameObject;
            Agent = GetComponent<NavMeshAgent>();
            _audio = FindObjectOfType<AudioSource>();
            _manager = FindObjectOfType<Manager>();
            PatrolPath = FindObjectOfType<Patrol>();
            playerCollider.Add(m_Player.GetComponentInChildren<Collider>());
        }

        void Update()
        {
            UpdateRobotState();
        }

        void UpdateRobotState()
        {
            EnemyAIState();

            switch (State)
            {
                case RobotState.Patrol:
                    UpdatePatrolDestination();
                    SetNavMeshDestination(NextPatrolPointPosition());
                    break;
                case RobotState.Follow:
                    SetNavMeshDestination(detectedPlayerPosition.position);
                    RotationControl();
                    break;
                case RobotState.Attack:
                    SetNavMeshDestination(detectedPlayerPosition.position);
                    RotationControl();
                    Shoot();
                    break;
            }

            DestroyGameObject();

            void EnemyAIState()
            {
                float sqrDetectionMagnitude = detectionMagnitude * detectionMagnitude;
                float halfDetectionMagnitude = sqrDetectionMagnitude / 2;
                float sqrEnemyToPlayerDistance = (m_Player.transform.position - WeaponRoot.position).sqrMagnitude;

                if (sqrDetectionMagnitude > sqrEnemyToPlayerDistance)
                {
                    gameObject.GetComponentInChildren<Animator>().enabled = false;
                    State = RobotState.Follow;
                    m_ThreatDetected = true;
                }

                if (halfDetectionMagnitude > sqrEnemyToPlayerDistance && State == RobotState.Follow &&
                    Health.PlayerHealth > 0 && Physics.Raycast(WeaponRoot.position, transform.forward))
                {
                    State = RobotState.Attack;
                }

                if ((sqrDetectionMagnitude * 2.5f) < sqrEnemyToPlayerDistance && m_ThreatDetected)
                {
                    State = RobotState.Patrol;
                    m_ThreatDetected = false;
                }
            }



            void RotationControl()
            {
                Vector3 directionToFace = Vector3.ProjectOnPlane((m_Player.transform.position - transform.position), Vector3.up).normalized;
                if (m_Player.transform.position.magnitude != 0)
                {
                    Quaternion rotationToFace = Quaternion.LookRotation(directionToFace);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotationToFace, 10f * Time.deltaTime);
                }
            }

            void Shoot()
            {
                if (Time.time >= nextUpdate && Health.PlayerHealth >= 0)
                {
                    nextUpdate = Mathf.FloorToInt(Time.time) + 1;
                    Instantiate(bullet, rootPosition.position, rootRotation.rotation);
                    _audio.PlayOneShot(shoot);
                }
            }

            bool IsPatrolValid()
            {
                return PatrolPath && PatrolPath.PatrolPoints.Count > 0;
            }

            Vector3 NextPatrolPointPosition()
            {
                if (IsPatrolValid())
                {
                    return PatrolPath.GetPositionOfPatrolPoint(m_PatrolPointIndexToMoveTo);
                }

                return transform.position;
            }

            void SetNavMeshDestination(Vector3 destination)
            {
                Agent.SetDestination(destination);
            }

            void UpdatePatrolDestination(bool inverseOrder = false)
            {
                if (IsPatrolValid())
                {
                    if ((transform.position - NextPatrolPointPosition()).magnitude <= PatrolReachingRadius)
                    {
                        m_PatrolPointIndexToMoveTo = inverseOrder
                            ? (m_PatrolPointIndexToMoveTo - 1)
                            : (m_PatrolPointIndexToMoveTo + 1);
                        if (m_PatrolPointIndexToMoveTo < 0)
                        {
                            m_PatrolPointIndexToMoveTo += PatrolPath.PatrolPoints.Count;
                        }

                        if (m_PatrolPointIndexToMoveTo >= PatrolPath.PatrolPoints.Count)
                        {
                            m_PatrolPointIndexToMoveTo -= PatrolPath.PatrolPoints.Count;
                        }
                    }
                }
            }

            void DestroyGameObject()
            {
                if (HoverBotHealth.EnemyHealth <= 0)
                {
                    Destroy(gameObject);
                    _manager.amountOfEnemyKilled++;
                }
            }
        }
    }
}