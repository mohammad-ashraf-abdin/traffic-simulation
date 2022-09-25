using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove1 : MonoBehaviour
{
    public Camera cam;
    public GameObject gg;
    public NavMeshAgent agent;
    public GameObject wayPoint;

    private void Start()
    {
        agent.SetDestination(wayPoint.transform.position);
    }

}
