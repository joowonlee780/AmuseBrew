using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    [Header("순찰할 지점")]
    public List<Transform> wayPoints;
    private int nextIdx;
    

    private readonly float patrollSpeed = 0.6f;
    private readonly float traceSpeed = 0.8f;
   
    private float damping = 1.0f;
    
    private NavMeshAgent agent;
    private Transform enemyTr;
    //public GameObject point;

    //public GameObject[] point = null; 
    
    private bool _patrolling;
    public bool patrolling
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrollSpeed;
                damping = 1.0f;
                MoveWayPoint();
            }
        }
    }

    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }

    public float speed
    {
        get { return agent.velocity.magnitude; }

    }

    void Start()
    {
        enemyTr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.updateRotation = false;
        agent.speed = patrollSpeed;
        
        //var group = GameObject.Find("WayPointGroup");
        GameObject group = null;
        // for (int i = 0; i < point.Length; i++)
        // {
        //     group = point[i];
        // }
        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoints);
            wayPoints.RemoveAt(0);
        }
        
        
        MoveWayPoint();
    }

    void MoveWayPoint()
    {
        if (agent.isPathStale) return;

        agent.destination = wayPoints[nextIdx].position;
        agent.isStopped = false;
    }

    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;

        agent.destination = pos;
        agent.isStopped = false;
    }

    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }
    void Update()
    {
         if (agent.isStopped == false)
         {
             Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
             enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
         }
         
        if (!_patrolling) 
            return;
        
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance < 0.5f)
        {
            nextIdx = ++nextIdx % wayPoints.Count;
            //nextIdx++;
            this.transform.LookAt(wayPoints[nextIdx]);
            MoveWayPoint();
        }
    }
}
