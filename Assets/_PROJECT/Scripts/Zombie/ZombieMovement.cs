using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class ZombieMovement : MonoBehaviour,IDamageable
{
    [Header("Movement")] 
    private NavMeshAgent _zombieAgent;

    private List<Transform> _waypoints = new();
    private int _currentWaypointIndex;
    private Transform _fireTransform;

    [SerializeField] private int moveToken;

    [SerializeField] private float destinationCheckTimer;
    
    [SerializeField] private ZombieDataObject zombieDataObject;

    private void Awake()
    {
        if (_zombieAgent == null)
        {
            _zombieAgent = GetComponent<NavMeshAgent>();
        }
    }

    private void Start()
    {
        _zombieAgent.speed = zombieDataObject.MoveSpeed;
        _zombieAgent.angularSpeed = zombieDataObject.TurnSpeed;
        _zombieAgent.acceleration = zombieDataObject.Acceleration;
        _zombieAgent.stoppingDistance = zombieDataObject.StoppingDistance;
        StartCoroutine(DestinationCheckRoutine());
    }
    
    public void SetWaypoints(Transform[] wayPoints, int spawnIndex, Transform fireTransform)
    {
        foreach (var waypoint in wayPoints)
        {
            _waypoints.Add(waypoint);
        }
        
        _zombieAgent.Warp(wayPoints[spawnIndex].position);
        
        _currentWaypointIndex = spawnIndex;
        
        _fireTransform = fireTransform;
    }

    private void SetNewWaypointIndex()
    {
        var oldWaypointIndex = _currentWaypointIndex;
        while (_currentWaypointIndex == oldWaypointIndex)
        {
            _currentWaypointIndex = oldWaypointIndex + Random.Range(-1, 1);
            if (_currentWaypointIndex < 0)
            {
                _currentWaypointIndex = _waypoints.Count - 1;
            }
            if (_currentWaypointIndex >= _waypoints.Count)
            {
                _currentWaypointIndex = 0;
            }
        }
    }
    private void SetNewDestination()
    {
        SetNewWaypointIndex();
        if (moveToken != 0)
        {
            _zombieAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
            moveToken--;
        }
        else
        {
            _zombieAgent.SetDestination(_fireTransform.position);
            _zombieAgent.stoppingDistance = 5;
        }
    }

    private IEnumerator DestinationCheckRoutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(destinationCheckTimer);
            if (_zombieAgent.remainingDistance <= _zombieAgent.stoppingDistance)
            {
                SetNewDestination();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        
    }
}
