using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class AIMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private List<Transform> waypoints;
    private int _currentWaypoint;
    [SerializeField] private float minimumDistance = 0.5f;
    [SerializeField] private float routineTimer = 1.5f;
    void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        StartCoroutine(TrackingRoutine());
    }

    private void Update()
    {
        if (!(agent.remainingDistance <= minimumDistance || _currentWaypoint >= waypoints.Count - 1)) return;
        _currentWaypoint++;
        StartCoroutine(TrackingRoutine());
    }
    public void SetNewDestination()
    {
        agent.SetDestination(waypoints[_currentWaypoint].position);
    }
    public void SetNewDestination(Transform destination)
    {
        agent.SetDestination(destination.position);
    }

    private IEnumerator TrackingRoutine()
    {
        SetNewDestination();
        while (agent.remainingDistance > minimumDistance)
        {
            yield return new WaitForSeconds(routineTimer);
            SetNewDestination();
        }
    }
}
